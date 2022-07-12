<template>
	<base-page>
		<div class=" h-full flex flex-col p-6">
			<div class="flex">
				<h1 class="text-3xl mb-6 flex-grow">
					{{ season.name }}
				</h1>
			</div>
			<div class="flex mb-10 w-full">
				<card class="card flex-grow p-3 mr-6">
					<spa-link target="/drivers">
						<h4 class="font-l"> Driver standings </h4>
					</spa-link>
				</card>
				<card class="card flex-grow p-3 mr-6" :disabled="true">
					<spa-link target="/teams" class="card">
						<h4 class="font-l"> Team standings </h4>
					</spa-link>
				</card>
			</div>
			<div class="grid grid-cols-3 w-full mb-6 gap-6">
				<card v-for="(p, i) in profiles" :key="p.driver.id">
					<spa-link :target="'drivers/' + p.driver.id" :reqData="{ seasonId: season.id}" class="flex flex-col p-4 text-2xl">
						<div class="flex fill-w items-center mb-1">
							<h3> #{{ i + 1 }} </h3>
							<div class="flex-grow text-end font-semibold">
								Points: <b>{{ p.seasonPoints }}</b>
							</div>
						</div>
						<h2>
							{{ p.driver.nickname }}
						</h2>
						<div class="flex-grow relative w-full h-64">
							<div class="absolute w-full h-full">
								<img class=" object-right-bottom object-contain w-full h-full" :src="p.profile.imageLink">
							</div>
							<span class="absolute left-0 bottom-0">
								<h1 class="text-6xl font-f1" :style="p.mostCommonTeam ? 'color: #' + p.mostCommonTeam.mainColor + '; text-shadow: 5px 5px #' + p.mostCommonTeam.secondaryColor : ''">{{ p.driver.number }}</h1>
							</span>
						</div>
					</spa-link>
				</card>
			</div>
			<h1 class="text-3xl mb-6" v-if="chartOk">
				Points Timeline
			</h1>
			<canvas class="w-full" id="driver-points-canvas">

			</canvas>
		</div>
	</base-page>
</template>

<script lang="ts">
import Section from '../layout/section.vue';
import DriverInfo from '@/data/DriverInfo';
import Season from '@/data/Season';
import SpaLink from '@/SPA/SpaLink.vue';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import BasePage from '../layout/basePage.vue';
import * as Chart from 'chart.js';

interface ProfileDataset {
	label: string,
	data: number[],
	fill: boolean,
	backgroundColor: string,
	borderColor: string,
	tension: number,
}

interface SeasonData {
	labels: string[],
	datasets: ProfileDataset[]
}

@Component({
	components: {
		BasePage,
		SpaLink,
		"card": Section	
	}
})
export default class extends Vue{
	@Prop({default: (): DriverInfo[] => null}) readonly profiles: DriverInfo[];
	@Prop({default: (): Season => null}) readonly season: Season;
	@Prop({default: (): Season[] => null}) readonly seasons: Season[];

	private _chart: Chart.Chart;

	chartOk() {
		return this._chart != null;
	}

	mounted() {
		let data: SeasonData = {
			labels: [],
			datasets:  [],
		};

		let profileDatasets = new Map<number, ProfileDataset>();


		if (this.profiles) {
			this.profiles.forEach((p) => {
				let profileDataset = {
					label: p.driver.nickname,
					data: new Array<number>(),
					fill: false,
					backgroundColor: '#' + p.mostCommonTeam.mainColor,
					borderColor: '#' + p.mostCommonTeam.mainColor,
					tension: 0.001,
					pointRadius: 5,
					pointHoverRadius: 8
				};
				
				data.datasets.push(profileDataset);
				profileDatasets.set(p.driver.id, profileDataset);
			});
		} else {
			return;
		}

		if (this.season.races) {
			for (let i = 0; i < this.season.races.length; i++) {
				let r = this.season.races[i];
				data.labels.push(r.name);

				if (r.raceResults) {
					r.raceResults.forEach(rr => {
						if (profileDatasets.has(rr.driverId)) {
							let dataSet = profileDatasets.get(rr.driverId);
							let points = rr.points;
							if (i > 0) {
								points += dataSet.data[i - 1];
							}
							dataSet.data.push(points);
						}
					});

					profileDatasets.forEach(val => {
						if (val.data.length < i + 1) {
							val.data.push(i > 0 ? val.data[i - 1] : 0);
						}
					});
				}
			}
		} else {
			console.log("No races in season");
			return;
		}

		Chart.Chart.register(Chart.LineController, Chart.CategoryScale, Chart.LinearScale, Chart.PointElement, Chart.LineElement, Chart.Tooltip);

		const chart = new Chart.Chart('driver-points-canvas', {
			type: "line",
			data: data,
			options: {
				plugins: {
					tooltip: {
						enabled: true,
						titleFont: {
							weight: 'bold',
							size: 16,
						},
						bodyFont: {
							size: 16
						},
						callbacks: {
							labelTextColor: function(context) {
								return context.dataset.borderColor?.toString() ?? '#FFF';
							},
							label: function(context) {
								let name = context.dataset.label ?? '<Driver>';

								return name + ' ' + context.dataset.data[context.dataIndex];
							}
						}
					}
				}
			}
		});

		this._chart = chart;
	}

	unmounted() {
		if (this._chart) {
			this._chart.destroy();
		}
	}
}
</script>