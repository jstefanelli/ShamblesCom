<template>
	<base-page>
		<div class="margin-fill-h padding-25 flex flex-vertical">
			<div class="flex margin-b-25">
				<div class="font-l">
					Season:
				</div>
				<select class="flex-grow margin-l-10" v-model="activeSeasonId">
					<option v-for="season in seasons"
						:key="season.id"
						:value="season.id">
						{{season.name}}
					</option>
				</select>
				<button v-on:click="$refs.newSeasonModal.open()" class="margin-l-10">
					Create new
				</button>
			</div>
			<div class="flex-grow flex align-center" v-if="!activeSeason">
				<div class="flex-grow" />
				<div class="font-l2">Select a season to start adding data</div>
				<div class="flex-grow" />
			</div>
			<season-display v-else :season="activeSeason">

			</season-display>
		</div>
		<new-season-modal ref="newSeasonModal" :categories="categories">

		</new-season-modal>
	</base-page>
</template>

<script lang="ts">
import Category from '@/data/Category';
import Game from '@/data/Game';
import Season from '@/data/Season';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import BasePage from '../layout/basePage.vue';
import NewSeasonModal from './modals/newSeasonModal.vue';
import SeasonDisplay from './seasonDisplay.vue';

@Component({
	components: {
		BasePage,
		NewSeasonModal,
		SeasonDisplay
	}
})
export default class extends Vue {
	@Prop({default: (): Season[] => { return [];} }) private seasons: Season[];
	@Prop({default: (): Game[] =>{ return [];} }) private games: Game[];
	@Prop({default: (): Category[] =>{ return [];} }) private categories: Category[];

	activeSeasonId: number = null;
	activeSeason: Season = null;
	showNewSeasonModal: boolean = false;

	@Watch("activeSeasonId")
	private seasonIdChanged(val: number, old: number) {
		this.seasons.forEach((s) => {
			if (s.id == val)
				this.activeSeason = s;
		})
	}

	private mounted(): void {

	}
}

</script>