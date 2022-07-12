<template>
	<modal ref="myModal">
		<template slot="header">
			{{ race && race.id != 0 ? "Edit Race:" : "Add a new race:" }}
		</template>

		<template>
			<div class="flex flex-col text-xl">
				<span v-if="errors && errors.Name" class="text-red-500 mb-3"><b>Error:</b> {{ errors.Name }}</span>
				<span>Name:</span>
				<Myinput type="text" class="my-3" v-model="name"></Myinput>

				<span v-if="errors && errors.TrackId" class="text-red-500 mb-3"><b>Error:</b> {{ errors.TrackId }}</span>
				<span>Track:</span>
				<Myselect v-model="trackId" class="my-3">
					<option v-for="t in tracks" :key="t.id" :value="t.id">
						{{ t.name }} ({{ t.country }})
					</option>
				</Myselect>

				<span v-if="errors && errors.DateTime" class="text-red-500 mb-3"><b>Error:</b> {{ errors.DateTime }}</span>
				<span>Start Date/Time:</span>
				<date-picker v-model="date" class="my-3" ></date-picker>

				<span v-if="errors && errors.LivestreamLink" class="text-red-500 mb-3"><b>Error:</b> {{ errors.LivestreamLink }}</span>
				<span>Stream Link:</span>
				<Myinput type="text" class="my-3" v-model="streamLink" ></Myinput>

				<span v-if="errors && errors.VodLink" class="text-red-500 mb-3"><b>Error:</b> {{ errors.VodLink }}</span>
				<span>VoD Link:</span>
				<Myinput type="text" class="my-3" v-model="vodLink" ></Myinput>
			</div>
		</template>

		<template slot="footer-heading">
			<Mybutton @click="deleteRace" v-if="race">
				Delete this race
			</Mybutton>
		</template>

		<template slot="footer">
			<Mybutton @click="close">
				Cancel
			</Mybutton>
			<Mybutton :main="true" @click="submit">
				Save
			</Mybutton>
		</template>
	</modal>
</template>

<script lang="ts">
import DatePicker from '@/components/layout/controls/DatePicker.vue';
import Modal from '@/components/modal/modal.vue';
import Race from '@/data/Race';
import Season from '@/data/Season';
import Track from '@/data/Track';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import Myinput from '@/components/layout/controls/myinput.vue';
import Myselect from '@/components/layout/controls/myselect.vue';
import Mybutton from '@/components/layout/controls/mybutton.vue';

@Component({
	components: {
		Modal,
		DatePicker,
		Myinput,
		Myselect,
		Mybutton
	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) public season: Season;
	@Prop({default: (): Track[] => null}) public tracks: Track[];
	@Prop({default: (): Race => null}) public race: Race;
	public errors: any = {};
	public name: string = "";
	public trackId: number = 0;
	public date: Date = null;
	public streamLink: string = '';
	public vodLink: string = '';

	public modal(): any {
		return (this.$refs.myModal as any);
	}

	@Watch("race")
	public raceChanged(val: Race, old: Race) {
		if (val) {
			this.name = val.name;
			this.trackId = val.trackId;
			this.date = new Date(val.dateTime);
			this.streamLink = val.livestreamLink;
			this.vodLink = val.vodLink;
		} else {
			this.name = '';
			this.trackId = 0;
			this.date = null;
			this.streamLink = '';
			this.vodLink = '';
		}
	}

	public submit(): void {
		let r: Race = {
			id: this.race ? this.race.id : 0,
			trackId: this.trackId,
			seasonId: this.season.id,
			dateTime: this.date ? this.date.toISOString() : "",
			name: this.name,
			livestreamLink: this.streamLink,
			vodLink: this.vodLink
		};

		if (r.id == 0) {
			SPA.navigateAndRender("/admin/" + this.season.id + "/races", "POST", r, true).then(() => {
				this.errors = {};
			}).catch((errors) => {
				if (typeof(errors) == 'object') {
					this.errors = errors.errors;
				}else {
					this.errors = {
						Name: "Server error"
					};
				}
			});
		} else {
			SPA.navigateAndRender("/admin/" + this.season.id + "/races/" + r.id, "PUT", r, true).then(() => {
				this.errors = {};
			}).catch((errors) => {
				if (typeof(errors) == 'object') {
					this.errors = errors.errors;
				} else {
					this.errors = {
						Name: "Server Error"
					};
				}
			});
		}
	}
	
	public deleteRace() {
		if (!this.race)
			return;

		SPA.navigateAndRender("/admin/" + this.season.id + "/races/" + this.race.id, "DELETE");
	}

	public close() {
		this.modal().closeModal();
	}

	public open() {
		this.modal().openModal();
	}
}
</script>