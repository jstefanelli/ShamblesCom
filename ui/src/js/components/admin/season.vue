<template>
	<base-layout v-bind="index">
		<div class="fill card no-hover flex flex-vertical padding-25">
			<div class="font-m margin-b-25">
				{{ season ? season.name : "unknown-season"}}
			</div>
			<div class="flex-grow flex">
				<button class="passive flex-grow margin-r-25 font-m" @click="viewRaces">
					Edit Races
				</button>
				<button class="passive flex-grow margin-r-25 font-m" @click="viewDrivers">
					Edit Drivers
				</button>
				<button class="passive flex-grow font-m" @click="$refs.editTeamsModal.open()">
					Edit Teams
				</button>
			</div>
			<edit-teams-modal :season="season" ref="editTeamsModal">

			</edit-teams-modal>
		</div>
	</base-layout>
</template>

<script lang="ts">
import SPA from '@/SPA/spa';
import Season from '@/data/Season';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import EditTeamsModal from './modals/editTeamsModal.vue';
import BaseLayout from './baseLayout.vue';

@Component({
	components: {
		EditTeamsModal,
		BaseLayout

	}
})
export default class extends Vue {
	@Prop({default: () : any => {} }) public index: any;
	@Prop({default: () : Season => null}) public season: Season;

	private viewRaces() {
		SPA.navigateAndRender("/admin/" + this.season.id + "/races");
	}

	private viewDrivers() {
		SPA.navigateAndRender("/admin/" + this.season.id + "/drivers");
	}
}
</script>