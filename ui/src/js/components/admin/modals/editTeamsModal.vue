<template>
	<modal ref="myModal">
		<template slot="header">
			'{{ season.name }}' Teams:
		</template>

		<template>
			<div class="flex flex-vertical">
				<div class="flex margin-v-10">
					<select class="flex-grow margin-h-10" v-model="currentTeamId">
						<option v-for="team in season.teams" :key="team.id" :value="team.id">
							{{ team.name }}
						</option>
					</select>
					<button @click="addTeam">
						New Team
					</button>
				</div>
				<div class="flex flex-vertical" v-if="currentTeam">
					<span v-if="errors && errors.Name" class="color-red margin-b-10"><b>Error:</b> {{ errors.Name }}</span>
					<span>Name:</span>
					<input type="text" class="margin-v-10" v-model="currentTeam.name"/>

					<span v-if="errors && errors.MainColor" class="colo-red margin-b-10"><b>Error:</b> {{ errors.MainColor }}</span>
					<span>Main Color:</span>
					<div class="flex margin-v-10">
						<input type="text" class="flex-grow margin-r-10" v-model="currentTeam.mainColor" />
						<div class="width-100" :style="'background-color: #' + this.currentTeam.mainColor" />
					</div>
					
					<span v-if="errors && errors.MainColor" class="colo-red margin-b-10"><b>Error:</b> {{ errors.MainColor }}</span>
					<span>Secondary Color:</span>
					<div class="flex margin-v-10">
						<input type="text" class="flex-grow margin-r-10" v-model="currentTeam.secondaryColor" />
						<div class="width-100" :style="'background-color: #' + this.currentTeam.secondaryColor" />
					</div>
				</div>
				<div class="flex flex-vertical align-center" v-else>
					<div class="flex-grow" />
					<span class="font-m">Select or create a team</span>
					<div class="flex-grow" />
				</div>
			</div>
		</template>

		<template slot="footer">
			<button @click="close()" class="passive">
				Cancel
			</button>
			<button @click="submit">
				Save
			</button>
		</template>
	</modal>
</template>

<script lang="ts">
import Modal from '@/components/modal/modal.vue';
import Team from '@/data/Team';
import Season from '@/data/Season';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
	components: {
		Modal
	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) public season: Season;
	private currentTeam: Team = null;
	private currentTeamId: number = 0;

	public errors: any = null;

	private addTeam() {
		this.currentTeamId = 0;
		this.currentTeam = {
			id: 0,
			name: 'New Team',
			seasonId: this.season.id,
			mainColor: "000",
			secondaryColor: "FFF",
			season: null
		};
	}

	@Watch("currentTeamId")
	private onCurrentTeamIdChanged(val: number, oldVal: number) {
		this.season.teams.forEach((t) => {
			if (t.id == val) {
				this.currentTeam = t;
			}
		})	
	}

	private modal(): any {
		return (this.$refs.myModal as any);
	}

	public open(): void {
		this.currentTeam = null;
		this.modal().openModal();
	}

	public close(): void {
		this.modal().closeModal();
	}

	public submit(): void {

		let json = JSON.stringify(this.currentTeam);

		if (this.currentTeam.id == 0) {
			fetch("/api/team", {
				method: "POST",
				headers: {
					'Content-Type': 'application/json',
					'Accept': 'application/json'
				},
				body: json,
				mode: 'cors'
			}).then((res) => {
				if (res.ok) {
					SPA.navigateAndRender("/admin/" + this.currentTeam.seasonId);
				} else {
					res.json().then((data) => this.errors = data.errors).catch((err) => {
						this.errors = { "Name" : "Server error"};
						console.warn("[AddTeam] Failed to add team. Late error:", err);
						return res.text()
					}).then(result => {
						console.warn("[AddTeam] Full result: ", result);
					});
				}
			}).catch((err) => {
				console.error("[AddTeam] Failed to add team: ", err);
			});
		}else {
			fetch("/api/team/" + this.currentTeam.id, {
				method: 'PUT',
				headers: {
					'Content-Type': 'application/json',
					'Accept': 'application/json'
				},
				body: json,
				mode: 'cors'
			}).then((res) => {
				if (res.ok) {
					SPA.navigateAndRender("/admin/" + this.currentTeam.seasonId);
				} else {
					res.json().then((data) => this.errors = data.errors).catch((err) => {
						this.errors = { "Name" : "Server error"};
						console.warn("[EditTeam] Failed to edit team. Late error:", err);
						return res.text()
					}).then(result => {
						console.warn("[EditTeam] Full result: ", result);
					});
				}
			}).catch((err) => {
				console.error("[EditTeam] Failed to edit team: ", err);
			})
		}
	}
}
</script>