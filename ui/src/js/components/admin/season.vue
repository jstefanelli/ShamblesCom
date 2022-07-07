<template>
	<base-layout v-bind="index">
		<card class="w-full h-full flex flex-col p-6">
			<div class="flex mb-6 items-center">
				<div class="font-m ">
					{{ season ? season.name : "unknown-season"}}
				</div>
				<div class="flex-grow">

				</div>
				<button class="passive" @click="showOneHomepage" v-if="settings && season && settings.currentHomeSeasonId != season.id">
					Display this season on the HomePage
				</button>
			</div>
			<div class="flex-grow flex">
				<button class="passive flex-grow margin-r-25 font-m" @click="viewRaces">
					Edit Races
				</button>
				<button class="passive flex-grow margin-r-25 font-m" @click="viewDrivers">
					Edit Drivers
				</button>
				<button class="passive flex-grow font-m" @click="($refs.editTeamsModal).open()">
					Edit Teams
				</button>
			</div>
			<edit-teams-modal :season="season" ref="editTeamsModal"></edit-teams-modal>
		</card>
	</base-layout>
</template>

<script lang="ts">
import Section from '../layout/section.vue';
import SPA from '@/SPA/spa';
import Season from '@/data/Season';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import EditTeamsModal from './modals/editTeamsModal.vue';
import BaseLayout from './baseLayout.vue';
import SiteSettings from '@/data/SiteSettings'

@Component({
	components: {
		EditTeamsModal,
		BaseLayout,
		"card": Section
	}
})
export default class extends Vue {
	@Prop({default: (): any => {} }) public index: any;
	@Prop({default: (): Season => null}) public season: Season;
	@Prop({default: (): SiteSettings => null }) public settings: SiteSettings;

	public viewRaces() {
		SPA.navigateAndRender("/admin/" + this.season.id + "/races");
	}

	public viewDrivers() {
		SPA.navigateAndRender("/admin/" + this.season.id + "/drivers");
	}

	showOneHomepage() {
		SPA.navigateAndRender("/admin/" + this.season.id + "/displaySeason", "POST");
	}
}
</script>