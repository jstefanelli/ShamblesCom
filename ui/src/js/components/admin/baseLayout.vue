<template>
	<base-page>
		<div class="margin-fill-h padding-25 flex flex-vertical">
			<div class="flex margin-b-25">
				<div class="font-l">
					Season:
				</div>
				<select class="flex-grow margin-l-10" v-model="activeSeasonId" @change="selectSeason">
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
			<div class="flex-grow">
				<slot>

				</slot>
			</div>
		</div>
		<new-season-modal ref="newSeasonModal" :categories="categories">

		</new-season-modal>
	</base-page>
</template>

<script lang="ts">
import Category from '@/data/Category';
import Game from '@/data/Game';
import Season from '@/data/Season';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import BasePage from '../layout/basePage.vue';
import NewSeasonModal from './modals/newSeasonModal.vue';

@Component({
	components: {
		BasePage,
		NewSeasonModal
	}
})
export default class extends Vue {
	@Prop({default: (): Season[] => [] }) private seasons: Season[];
	@Prop({default: (): Game[] => [] }) private games: Game[];
	@Prop({default: (): Category[] => [] }) private categories: Category[];
	@Prop({default: 0}) private selectedSeasonId: number;

	activeSeasonId: number = null;
	showNewSeasonModal: boolean = false;

	private selectSeason() {
		SPA.navigateAndRender("/admin/" + this.activeSeasonId);
	}

	private mounted(): void {
		if (typeof(this.selectedSeasonId) == 'number') {
			this.activeSeasonId = this.selectedSeasonId;
		}
	}
}

</script>