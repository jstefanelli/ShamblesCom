<template>
	<base-page>
		<div class="margin-fill-h padding-25 flex flex-vertical">
			<div class="flex">
				<div class="font-l">
					Season:
				</div>
				<select class="flex-grow margin-l-25" v-model="activeSeason">
					<option v-for="season in seasons"
						:key="season.id"
						:value="season">
						{{season.name}}
					</option>
				</select>
			</div>
			<div class="flex-grow flex align-center" v-if="!activeSeason">
				<div class="flex-grow" />
				<div class="font-xl">Select a season to start adding data</div>
				<div class="flex-grow" />
			</div>
		</div>
	</base-page>
</template>

<script lang="ts">
import Season from '@/data/Season';
import { Vue, Component, Prop } from 'vue-property-decorator';
import BasePage from '../layout/basePage.vue';

@Component({
	components: {
		BasePage,
	}
})
export default class extends Vue {
	@Prop({default: (): Season[] => { return [];} }) private seasons: Season[];

	activeSeason: Season = null;
}

</script>