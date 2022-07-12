<template>
	<base-page>
		<div class="p-6 flex flex-col">
			<div class="flex items-center mb-6">
				<div class="text-2xl">
					Season:
				</div>
				<Myselect class="flex-grow ml-3" v-model="activeSeasonId" @change="selectSeason">
					<option v-for="season in seasons"
						:key="season.id"
						:value="season.id">
						{{season.name}}
					</option>
				</Myselect>
				<my-button v-on:click="openModal()" class="ml-3">
					Create new
				</my-button>
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
import Button from '../layout/controls/mybutton.vue';
import Myselect from '../layout/controls/myselect.vue';

@Component({
	components: {
		BasePage,
		NewSeasonModal,
		"my-button": Button,
		Myselect
	}
})
export default class extends Vue {
	@Prop({default: (): Season[] => [] }) public seasons: Season[];
	@Prop({default: (): Game[] => [] }) public games: Game[];
	@Prop({default: (): Category[] => [] }) public categories: Category[];
	@Prop({default: 0}) public selectedSeasonId: number;

	activeSeasonId: number = null;
	showNewSeasonModal: boolean = false;

	public selectSeason() {
		SPA.navigateAndRender("/admin/" + this.activeSeasonId);
	}

	public openModal() {
		(this.$refs.newSeasonModal as any).open();
	}

	private mounted(): void {
		if (typeof(this.selectedSeasonId) == 'number') {
			this.activeSeasonId = this.selectedSeasonId;
		}
	}
}

</script>