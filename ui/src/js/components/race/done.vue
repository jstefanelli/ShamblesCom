<template>
	<div class="flex flex-col items-left gap-6 py-12 h-full">
		<div class="flex flex-row items-center gap-2">
			<span class="text-5xl text-white font-f1 flex-grow">
				{{ race.name }}
			</span>
			<spa-link :target="'/season/' + race.seasonId" class="text-red-titlebar font-bold text-xl">
				View season
			</spa-link>
		</div>
		<div class="w-full flex-grow flex flex-row gap-5">
			<div class="flex-grow overflow-y-auto flex flex-col gap-3 px-5">
				<div class="flex flex-row gap-3">
					<card class="text-3xl font-bold w-1/2 cursor-pointer p-3 text-white" :disabled="displayStartingGrid" @click="displayStartingGrid = false">
						Results
					</card>
					<card class="text-3xl font-bold w-1/2 cursor-pointer p-3 text-white" :disabled="!displayStartingGrid" @click="displayStartingGrid = true">
						Starting Grid
					</card>
				</div>
				<card class="flex overflow-y-auto flex-col gap-3 p-5">
					<div class="flex flex-col gap-6">
						<div class="flex p-5 rounded-md bg-bg-second border-2 border-white text-3xl items-center" v-for="r in race.raceResults" v-if="race && race.raceResults && !displayStartingGrid" :key="r.id">
							<span class="font-f1 min-w-[10rem] text-3xl" :style="'color: #' + r.team.mainColor + '; text-shadow: 3px 3px #' + r.team.secondaryColor + ';'">
								{{ r.finished ? '#' + r.position : 'DNF' }}
							</span>
							<span class="font-f1 text-red-titlebar flex-grow text-center">
								<spa-link :target="'/drivers/' + r.driverId"> {{ r.driver.nickname }} </spa-link>
							</span>
							<span class=" text-2xl min-w-[10rem] text-right">
								Points: {{ r.points }}
							</span>
						</div>
						<div class="flex p-5 rounded-md bg-bg-second border-2 border-white text-3xl items-center" v-for="r in startResults" v-if="displayStartingGrid && startResults" :key="r.id">
							<span class="font-f1 min-w-[10rem] text-3xl" :style="'color: #' + r.team.mainColor + '; text-shadow: 3px 3px #' + r.team.secondaryColor + ';'">
								{{ '#' + r.startPosition }}
							</span>
							<span class="font-f1 text-red-titlebar flex-grow text-center">
								<spa-link :target="'/drivers/' + r.driverId"> {{ r.driver.nickname }} </spa-link>
							</span>
						</div>
						
					</div>
				</card>
			</div>
			<card class=" min-w-[25rem] overflow-y-auto flex flex-col gap-3 p-5 text-lg">
				<span class="text-3xl font-bold">
					Info
				</span>
				<span>
					<b>Broadcasted on: </b> {{ new Date(race.dateTime).toLocaleString() }}
				</span>
				<span v-if="race.track && race.track.name && race.track.name.length > 0">
					<b>Track: </b>{{ race.track.name }}
				</span>
				<span v-if="race.vodLink && race.vodLink.length > 0">
					<b>VoD available: </b> <a class="text-red-titlebar" :href="race.vodLink">here</a>
				</span>
			</card>
		</div>
	</div>
</template>

<script lang="ts">
import Race from '@/data/Race';
import { Vue, Prop, Watch, Component } from 'vue-property-decorator';
import Card from '@/components/layout/section.vue';
import SpaLink from '@/SPA/SpaLink.vue';
import DriverCard from '@/components/layout/controls/driverCard.vue';
import RaceResult from '@/data/RaceResult';

@Component({
	components: {
		Card,
		SpaLink,
		DriverCard
	}
})
export default class extends Vue {
	@Prop({default: (): Race => null}) public readonly race: Race;

	public displayStartingGrid = false;

	public startResults: RaceResult[] = [];

	public mounted() {
		this.startResults = [...this.race.raceResults];
		this.startResults.sort((r0, r1) => {
			return r0.startPosition - r1.startPosition;
		})
	}
}

</script>