<template>
	<modal ref="myModal">
		<template slot="header">
			{{ race && race.id != 0 ? "Edit Race:" : "Add a new race:" }}
		</template>

		<template>
			<div class="flex flex-vertical">
				<span v-if="errors && errors.Name" class="color-red margin-b-10"><b>Error:</b> {{ errors.Name }}</span>
				<span>Name:</span>
				<input type="text" class="margin-v-10" v-model="name"/>

				<span v-if="errors && errors.TrackId" class="color-red margin-b-10"><b>Error:</b> {{ errrors.TrackId }}</span>
				<span>Track:</span>
				<select v-model="trackId" class="margin-v-10">
					<option v-for="t in tracks" :key="t.id" :value="t.id">
						{{ t.name }} ({{ t.country }})
					</option>
				</select>

				<span v-if="errors && errors.DateTime" class="color-red margin-b-10"><b>Error:</b> {{ errors.DateTime }}</span>
				<span>Start Date/Time:</span>
				<date-picker v-model="date" class="margin-v-10" />

				<span v-if="errors && errors.LivestreamLink" class="color-red margin-b-10"><b>Error:</b> {{ errors.LivestreamLink }}</span>
				<span>Stream Link:</span>
				<input type="text" class="margin-v-10" v-model="streamLink" />

				<span v-if="errors && errors.VodLink" class="color-red margin-b-10"><b>Error:</b> {{ errors.VodLink }}</span>
				<span>VoD Link:</span>
				<input type="text" class="margin-v-10" v-model="vodLink" />
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
import DatePicker from '@/components/layout/controls/DatePicker.vue';
import Modal from '@/components/modal/modal.vue';
import Race from '@/data/Race';
import Season from '@/data/Season';
import Track from '@/data/Track';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
	components: {
		Modal,
		DatePicker
	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) private season: Season;
	@Prop({default: (): Track[] => null}) private tracks: Track[];
	@Prop({default: (): Race => null}) private race: Race;
	private errors: any = {};
	private name: string = "";
	private trackId: number = 0;
	private date: Date = null;
	private streamLink: string = '';
	private vodLink: string = '';

	private modal(): any {
		return (this.$refs.myModal as any);
	}

	@Watch("race")
	private raceChanged(val: Race, old: Race) {
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

	private close() {
		this.modal().closeModal();
	}

	public open() {
		this.modal().openModal();
	}
}
</script>