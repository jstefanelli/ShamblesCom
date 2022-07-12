<template>
	<modal ref="myModal">
		<template slot="header">
			{{ result && result.id != 0 ? "Edit Result:" : "Add a new Result:" }}
		</template>

		<template>
			<div class="flex flex-col">
				<error-display v-if="errors && errors.DriverId" :errors="errors.DriverId" ></error-display>
				<span>Driver:</span>
				<Myselect v-model="driverId" class="my-3">
					<option v-for="d in drivers" :key="d.id" :value="d.id">
						{{ d.nickname }}
					</option>
				</Myselect>

				<error-display v-if="errors && errors.TeamId" :errors="errors.TeamId" ></error-display>
				<span>Team:</span>
				<Myselect v-model="teamId" class="my-3">
					<option v-for="t in teams" :key="t.id" :value="t.id">
						{{ t.name }}
					</option>
				</Myselect>
				
				<error-display v-if="errors && errors.Position" :errors="errors.Position" ></error-display>
				<span>Final Position:</span>
				<Myinput type="number" v-model.number="position" class="mt-3" ></Myinput>

				<error-display v-if="errors && errors.Finished" :errors="errors.Finished" ></error-display>
				<div class="flex my-3">
					<input type="checkbox" v-model="finished" >
					<span>Finished</span>
				</div>
				
				<error-display v-if="errors && errors.Points" :errors="errors.Points" ></error-display>
				<span>Points:</span>
				<Myinput type="number" v-model.number="points" class="my-3" ></Myinput>
				
				<error-display v-if="errors && errors.StartPosition" :errors="errors.StartPosition" ></error-display>
				<span>Start Position:</span>
				<Myinput type="number" v-model.number="startPosition" class="my-3" ></Myinput>
				
				<error-display v-if="errors && errors.Penalties" :errors="errors.Penalties" ></error-display>
				<span>Penalties:</span>
				<Myinput type="number" v-model.number="penalties" class="my-3" ></Myinput>
			</div>
		</template>

		<template slot="footer">
			<Mybutton @click="close">
				Cancel
			</Mybutton>
			<Mybutton @click="submit" :main="true">
				Save
			</Mybutton>
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
import Myselect from '@/components/layout/controls/myselect.vue';
import Myinput from '@/components/layout/controls/myinput.vue';
import Mybutton from '@/components/layout/controls/mybutton.vue';

@Component({
	components: {
		Modal,
		ErrorDisplay,
		Myselect,
		Myinput,
		Mybutton
	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) public season: Season;
	@Prop({default: (): Race => null}) public race: Race;
	@Prop({default: (): RaceResult => null}) public result: RaceResult;
	@Prop({default: (): Driver[] => []}) public drivers: Driver[];
	@Prop({default: (): Team[] => []}) public teams: Team[];

	public driverId: number = 0;
	public teamId: number = 0;
	public finished: boolean = false;
	public points: number = 0;
	public startPosition: number = 0;
	public penalties: number = 0;
	public position: number = 0;

	public errors: any = {};

	private modal(): any {
		return (this.$refs.myModal as any);
	}

	@Watch("result")
	public resultChanged(val: RaceResult, old: RaceResult) {
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

	public close() {
		this.modal().closeModal();
	}

	public open() {
		this.modal().openModal();
	}
}
</script>