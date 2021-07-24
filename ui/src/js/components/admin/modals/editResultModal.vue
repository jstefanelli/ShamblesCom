<template>
	<modal ref="myModal">
		<template slot="header">
			{{ result && result.id != 0 ? "Edit Result:" : "Add a new Result:" }}
		</template>

		<template>
			<div class="flex flex-vertical">
				<error-display v-if="errors && errors.DriverId" :errors="errors.DriverId" />
				<span>Driver:</span>
				<select v-model="driverId" class="margin-v-10">
					<option v-for="d in drivers" :key="d.id" :value="d.id">
						{{ d.nickname }}
					</option>
				</select>

				<error-display v-if="errors && errors.TeamId" :errors="errors.TeamId" />
				<span>Team:</span>
				<select v-model="teamId" class="margin-v-10">
					<option v-for="t in teams" :key="t.id" :value="t.id">
						{{ t.name }}
					</option>
				</select>
				
				<error-display v-if="errors && errors.Position" :errors="errors.Position" />
				<span>Final Position:</span>
				<input type="number" v-model.number="position" class="margin-t-10" />

				<error-display v-if="errors && errors.Finished" :errors="errors.Finished" />
				<div class="flex margin-v-10">
					<input type="checkbox" v-model="finished" />
					<span>Finished</span>
				</div>
				
				<error-display v-if="errors && errors.Points" :errors="errors.Points" />
				<span>Points:</span>
				<input type="number" v-model.number="points" class="margin-v-10" />
				
				<error-display v-if="errors && errors.StartPosition" :errors="errors.StartPosition" />
				<span>Start Position:</span>
				<input type="number" v-model.number="startPosition" class="margin-v-10" />
				
				<error-display v-if="errors && errors.Penalties" :errors="errors.Penalties" />
				<span>Penalties:</span>
				<input type="number" v-model.number="penalties" class="margin-v-10" />
			</div>
		</template>

		<template slot="footer">
			<button @click="close" class="passive">
				Cancel
			</button>
			<button @click="submit">
				Save
			</button>
		</template>
	</modal>
</template>

<script lang="ts">
import ErrorDisplay from '@/components/common/ErrorDisplay.vue';
import Modal from '@/components/modal/modal.vue';
import Driver from '@/data/Driver';
import Race from '@/data/Race';
import RaceResult from '@/data/RaceResult';
import Season from '@/data/Season';
import Team from '@/data/Team';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
	components: {
		Modal,
ErrorDisplay,
	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) private season: Season;
	@Prop({default: (): Race => null}) private race: Race;
	@Prop({default: (): RaceResult => null}) private result: RaceResult;
	@Prop({default: (): Driver[] => []}) private drivers: Driver[];
	@Prop({default: (): Team[] => []}) private teams: Team[];

	private driverId: number = 0;
	private teamId: number = 0;
	private finished: boolean = false;
	private points: number = 0;
	private startPosition: number = 0;
	private penalties: number = 0;
	private position: number = 0;

	private errors: any = {};

	private modal(): any {
		return (this.$refs.myModal as any);
	}

	@Watch("result")
	private resultChanged(val: RaceResult, old: RaceResult) {
		if (val) {
			this.driverId = val.driverId;
			this.teamId = val.teamId;
			this.finished = val.finished;
			this.points = val.points;
			this.startPosition = val.startPosition;
			this.penalties = val.penalties;
			this.position = val.position;
		} else {
			this.driverId = 0;
			this.teamId = 0;
			this.finished = false;
			this.points = 0;
			this.startPosition = 0;
			this.penalties = 0;
			this.position = 0;
		}
	}

	public submit(): void {
		let rr: RaceResult = {
			id: this.result ? this.result.id : 0,
			raceId: this.race.id,
			teamId: this.teamId,
			driverId: this.driverId,
			finished: this.finished,
			points: this.points,
			startPosition: this.startPosition,
			penalties: this.penalties,
			position: this.position,
		}

		if (rr.id == 0) {
			SPA.navigateAndRender("/admin/" + this.season.id + "/races/" + this.race.id + "/results", "POST", rr, true).then(() => {
				this.errors = {};
			}).catch((errors) => {
				if (typeof(errors) == 'object') {
					if (typeof(errors.erroors) == 'object') {
						this.errors = errors.errors;
					} else {
						this.errors = errors;
					}
				}else {
					this.errors = {
						Name: "Server error"
					};
				}
			});
		} else {
			SPA.navigateAndRender("/admin/" + this.season.id + "/races/" + this.race.id + "/results/" + rr.id, "PUT", rr, true).then(() => {
				this.errors = {};
			}).catch((errors) => {
				if (typeof(errors) == 'object') {
					if (typeof(errors.erroors) == 'object') {
						this.errors = errors.errors;
					} else {
						this.errors = errors;
					}
				} else {
					this.errors = {
						Name: "Server Error"
					};
				}
			});
		}
	}

	private close() {
		this.modal().closeModal();
	}

	public open() {
		this.modal().openModal();
	}
}
</script>