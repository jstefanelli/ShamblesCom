<template>
	<base-layout v-bind="index">
		<card class="w-full h-full flex flex-col p-6">
			<div class="flex mb-6 items-center">
				<div class="text-3xl ">
					{{ season ? season.name : "unknown-season"}}
				</div>
				<div class="flex-grow">

				</div>
				<Mybutton @click="showOneHomepage" v-if="settings && season && settings.currentHomeSeasonId != season.id">
					Display this season on the HomePage
				</Mybutton>
			</div>
			<div class="flex-grow flex">
				<Mybutton class="passive flex-grow mr-6 text-xl" @click="viewRaces">
					Edit Races
				</Mybutton>
				<Mybutton class="passive flex-grow mr-6 text-xl" @click="viewDrivers">
					Edit Drivers
				</Mybutton>
				<Mybutton class="passive flex-grow text-xl" @click="openModal">
					Edit Teams
				</Mybutton>
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
import Mybutton from '../layout/controls/mybutton.vue';

@Component({
	components: {
		EditTeamsModal,
		BaseLayout,
		"card": Section,
		Mybutton
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

	public openModal() {
		(this.$refs.editTeamsModal as any).open();
	}

	showOneHomepage() {
		SPA.navigateAndRender("/admin/" + this.season.id + "/displaySeason", "POST");
	}
}
</script>