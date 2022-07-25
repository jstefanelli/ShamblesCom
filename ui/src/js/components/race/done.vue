<template>
	<div class="flex flex-col items-left gap-6 py-12 h-full">
		<div class="flex flex-row items-center">
			<span class="text-5xl text-white flex-grow font-f1">
				{{ race.name }}
			</span>
			<spa-link :target="'/season/' + race.seasonId" class="text-red-titlebar font-bold text-xl">
				View season
			</spa-link>
		</div>
		<card class="w-full flex-grow overflow-y-auto flex flex-col gap-3 p-5">
			<span class="text-3xl font-bold">
				Results
			</span>
			<div class="h-80 grid grid-cols-3 gap-6">
				<driver-card title="#1" :result="race.raceResults[0]" v-if="race.raceResults && race.raceResults.length > 0" />
				<driver-card title="#2" :result="race.raceResults[1]" v-if="race.raceResults && race.raceResults.length > 1" />
				<driver-card title="#3" :result="race.raceResults[3]" v-if="race.raceResults && race.raceResults.length > 2" />
			</div>
		</card>
	</div>
</template>

<script lang="ts">
import Race from '@/data/Race';
import { Vue, Prop, Watch, Component } from 'vue-property-decorator';
import Card from '@/components/layout/section.vue';
import SpaLink from '@/SPA/SpaLink.vue';
import DriverCard from '@/components/layout/controls/driverCard.vue';

@Component({
	components: {
		Card,
		SpaLink,
		DriverCard
	}
})
export default class extends Vue {
	@Prop({default: (): Race => null}) public readonly race: Race;

}

</script>