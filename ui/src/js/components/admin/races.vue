<template>
	<base-layout v-bind="index">
		<card class="w-full h-full flex flex-col p-6">
			<div class="flex mb-6 items-center">
				<Mybutton class="mr-6 passive" @click="goUp">
					Back
				</Mybutton>
				<div class="font-m mr-3">
					Race:
				</div>
				<Myselect class="flex-grow h-full mr-3" @change="onRaceChanged" v-model="internalSelectedRaceId">
					<option v-for="race in season.races" :key="race.id" :value="race.id">
						{{ race.name }}
					</option>
				</Myselect>
				<Mybutton @click="showEditRaceDialog" class="mr-3">
					Edit Race
				</Mybutton>
				<Mybutton :main="true" @click="showNewRaceDialog">
					New Race
				</Mybutton>
			</div>
			<div class="flex-grow flex items-center" v-if="!selectedRace">
				<div class="text-center font-m flex-grow">
					Select or create a race
				</div>
			</div>
			<div class="flex-grow" v-else>
				<div class="flex flex-col w-full h-full">
					<div class="flex-grow">
						<table class="w-full">
							<thead>
								<th class="w-28">
									#
								</th>
								<th>
									Driver
								</th>
								<th class="w-56">
									Team
								</th>
								<th class="w-28">
									Points
								</th>
								<th class="w-56">
									Edit
								</th>
							</thead>
							<tbody class="overflow-y-auto">
								<tr v-for="rr in results" :key="rr.id">
									<td class="text-center">
										{{ rr.finished ? rr.position : "DNF" }}
									</td>
									<td class="text-center">
										{{ rr.driver.nickname }}
									</td>
									<td class="text-center">
										{{ rr.team.name}}
									</td>
									<td class="text-center">
										{{ rr.points }}
									</td>
									<td class="text-center">
										<Mybutton @click="editResult(rr)">
											Edit
										</Mybutton>
									</td>
								</tr>
							</tbody>
						</table>
					</div>
					<div class="flex">
						<div class="flex-grow" ></div>
						<Mybutton @click="addResult">
							Add Result
						</Mybutton>
					</div>
				</div>
			</div>
		</card>

		<edit-race-modal :season="season" :tracks="tracks" ref="new_race_modal" :race="editingRace" ></edit-race-modal>
		<edit-result-modal :season="season" :teams="season.teams" :drivers="drivers" :result="activeResult" :race="selectedRace" ref="edit_result_modal" ></edit-result-modal>
	</base-layout>
</template>

<script lang="ts">
import Driver from '@/data/Driver';
import Race from '@/data/Race';
import RaceResult from '@/data/RaceResult';
import Season from '@/data/Season';
import Track from '@/data/Track';
import { SPA } from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import BaseLayout from './baseLayout.vue';
import EditRaceModal from './modals/editRaceModal.vue';
import EditResultModal from './modals/editResultModal.vue';
import Section from '../layout/section.vue';
import Mybutton from '../layout/controls/mybutton.vue';
import Myselect from '../layout/controls/myselect.vue';

@Component({
	components: {
		BaseLayout,
		EditRaceModal,
		EditResultModal,
		'card': Section,
		Mybutton,
		Myselect
	}
})
export default class extends Vue {
	@Prop({default: (): any => {} }) public index: any;
	@Prop({default: (): Season => null }) public season: Season;
	@Prop({default: (): Track[] => [] }) public tracks: Track[];
	@Prop({default: (): RaceResult[] => []}) public results: RaceResult[]
	@Prop({default: 0}) public selectedRaceId: number;
	@Prop({default: (): Driver[] => []}) public drivers: Driver[]

	public activeResult: RaceResult = null;
	public internalSelectedRaceId: number = 0;
	public selectedRace: Race = null;
	public editingRace: Race = null;

	public goUp(): void {
		SPA.navigateAndRender("/admin/" + this.season.id);
	}

	public showNewRaceDialog(): void {
		this.editingRace = null;
		this.$nextTick(() => {
			(this.$refs.new_race_modal as any).open();
		});
	}

	public showEditRaceDialog(): void {
		if (!this.selectedRace) {
			return;
		}

		this.editingRace = this.selectedRace;
		this.$nextTick(() => {
			(this.$refs.new_race_modal as any).open();
		});
	}

	public onRaceChanged(ev: Event) {
		SPA.navigateAndRender("/admin/" + this.season.id + "/races/" + this.internalSelectedRaceId);
	}

	private mounted() {
		this.internalSelectedRaceId = this.selectedRaceId;
		var found = false;
			if(this.season.races) {
			this.season.races.forEach((r) => {
				if (r.id == this.selectedRaceId) {
					this.selectedRace = r;
					found = true;
				}
			});
		}

		if (!found)
			this.selectedRace = null;
	}

	public editResult(r: RaceResult) {
		this.activeResult = r;
		(this.$refs["edit_result_modal"] as any).open()
	}

	public addResult() {
		this.activeResult = null;
		(this.$refs["edit_result_modal"] as any).open();
	}
}
</script>	