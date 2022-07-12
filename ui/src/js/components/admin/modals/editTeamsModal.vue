<template>
	<modal ref="myModal">
		<template slot="header">
			'{{ season.name }}' Teams:
		</template>

		<template>
			<div class="flex flex-col">
				<div class="flex my-3">
					<Myselect class="flex-grow mr-3" v-model="currentTeamId">
						<option v-for="team in season.teams" :key="team.id" :value="team.id">
							{{ team.name }}
						</option>
					</Myselect>
					<Mybutton @click="addTeam">
						New Team
					</Mybutton>
				</div>
				<div class="flex flex-col" v-if="currentTeam">
					<span v-if="errors && errors.Name" class="text-red-500 mb-3"><b>Error:</b> {{ errors.Name }}</span>
					<span>Name:</span>
					<Myinput type="text" class="my-3" v-model="currentTeam.name"></Myinput>

					<span v-if="errors && errors.MainColor" class="text-red-500 mb-3"><b>Error:</b> {{ errors.MainColor }}</span>
					<span>Main Color:</span>
					<div class="flex my-3">
						<Myinput type="text" class="flex-grow mr-3" v-model="currentTeam.mainColor" ></Myinput>
						<div class="w-28" :style="'background-color: #' + currentTeam.mainColor" ></div>
					</div>
					
					<span v-if="errors && errors.MainColor" class="colo-red margin-b-10"><b>Error:</b> {{ errors.MainColor }}</span>
					<span>Secondary Color:</span>
					<div class="flex my-3">
						<Myinput type="text" class="flex-grow mr-3" v-model="currentTeam.secondaryColor" ></Myinput>
						<div class="w-28" :style="'background-color: #' + currentTeam.secondaryColor" ></div>
					</div>
				</div>
				<div class="flex flex-col items-center" v-else>
					<div class="flex-grow" ></div>
					<span class="font-m">Select or create a team</span>
					<div class="flex-grow" ></div>
				</div>
			</div>
		</template>

		<template slot="footer">
			<Mybutton @click="close()">
				Cancel
			</Mybutton>
			<Mybutton @click="submit" :main="true">
				Save
			</Mybutton>
		</template>
	</modal>
</template>

<script lang="ts">
import Modal from '@/components/modal/modal.vue';
import Team from '@/data/Team';
import Season from '@/data/Season';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import Myselect from '@/components/layout/controls/myselect.vue';
import Mybutton from '@/components/layout/controls/mybutton.vue';
import Myinput from '@/components/layout/controls/myinput.vue';

@Component({
	components: {
		Modal,
		Myselect,
		Mybutton,
		Myinput
	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) public season: Season;
	public currentTeam: Team = null;
	public currentTeamId: number = null;

	public errors: any = null;

	public addTeam() {
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
	public onCurrentTeamIdChanged(val: number, oldVal: number) {
		this.season.teams.forEach((t) => {
			if (t.id == val) {
				this.currentTeam = t;
			}
		})	
	}

	public modal(): any {
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