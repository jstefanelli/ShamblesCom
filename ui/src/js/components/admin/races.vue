<template>
	<base-layout v-bind="index">
		<div class="fill card no-hover flex flex-vertical padding-25">
			<div class="flex margin-b-25 align-center">
				<button class="margin-r-25 passive" @click="goUp">
					Back
				</button>
				<div class="font-m margin-r-10">
					Race:
				</div>
				<select class="flex-grow fill-h margin-r-10" @change="onRaceChanged" v-model="internalSelectedRaceId">
					<option v-for="race in season.races" :key="race.id" :value="race.id">
						{{ race.name }}
					</option>
				</select>
				<button @click="showEditRaceDialog" class="margin-r-10">
					Edit Race
				</button>
				<button @click="showNewRaceDialog" class="passive">
					New Race
				</button>
			</div>
			<div class="flex-grow flex align-center" v-if="!selectedRace">
				<div class="text-center font-m flex-grow">
					Select or create a race
				</div>
			</div>
			<div class="flex-grow" v-else>
				<div class="flex flex-vertical fill-w fill-h">
					<div class="flex-grow">
						<table class="fill-w">
							<thead>
								<th class="width-100">
									#
								</th>
								<th>
									Driver
								</th>
								<th class="width-200">
									Team
								</th>
								<th class="width-100">
									Points
								</th>
								<th class="width-200">
									Edit
								</th>
							</thead>
							<tbody class="overflow-y-auto">
								<tr v-for="rr in results" :key="rr.id">
									<td>
										{{ rr.finished ? rr.position : "DNF" }}
									</td>
									<td>
										{{ rr.driver.nickname }}
									</td>
									<td>
										{{ rr.team.name}}
									</td>
									<td>
										{{ rr.points }}
									</td>
									<td>
										<button @click="editResult(rr)">
											Edit
										</button>
									</td>
								</tr>
							</tbody>
						</table>
					</div>
					<div class="flex">
						<div class="flex-grow" />
						<button class="passive" @click="addResult">
							Add Resiòt
						</button>
					</div>
				</div>
			</div>
		</div>

		<edit-race-modal :season="season" :tracks="tracks" ref="new_race_modal" :race="editingRace" />
		<edit-result-modal :season="season" :teams="season.teams" :drivers="drivers" :result="activeResult" :race="selectedRace" ref="edit_result_modal" />
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

@Component({
	components: {
		BaseLayout,
		EditRaceModal,
		EditResultModal
	}
})
export default class extends Vue {
	@Prop({default: (): any => {} }) private index: any;
	@Prop({default: (): Season => null }) private season: Season;
	@Prop({default: (): Track[] => [] }) private tracks: Track[];
	@Prop({default: (): RaceResult[] => []}) private results: RaceResult[]
	@Prop({default: 0}) private selectedRaceId: number;
	@Prop({default: (): Driver[] => []}) private drivers: Driver[]

	private activeResult: RaceResult = null;
	private internalSelectedRaceId: number = 0;
	private selectedRace: Race = null;
	private editingRace: Race = null;

	private goUp(): void {
		SPA.navigateAndRender("/admin/" + this.season.id);
	}

	private showNewRaceDialog(): void {
		this.editingRace = null;
		this.$nextTick(() => {
			(this.$refs.new_race_modal as any).open();
		});
	}

	private showEditRaceDialog(): void {
		if (!this.selectedRace) {
			return;
		}

		this.editingRace = this.selectedRace;
		this.$nextTick(() => {
			(this.$refs.new_race_modal as any).open();
		});
	}

	private onRaceChanged(ev: Event) {
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

	private editResult(r: RaceResult) {
		this.activeResult = r;
		(this.$refs["edit_result_modal"] as any).open()
	}

	private addResult() {
		this.activeResult = null;
		(this.$refs["edit_result_modal"] as any).open();
	}
}
</script>	