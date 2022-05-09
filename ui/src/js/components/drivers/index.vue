<template>
	<base-page>
		<div class="padding-25 fill-h flex flex-vertical">
			<h1 class="font-l2 margin-b-25">
				{{ season.name }}
			</h1>
			<div class="flex margin-b-25 fill-w margin-b-40">
				<spa-link target="/drivers" class="card flex-grow padding-10 margin-r-25 plain">
					<h4 class="font-l"> Driver standings </h4>
				</spa-link>
				<spa-link target="/teams" class="card flex-grow padding-10 disabled plain">
					<h4 class="font-l"> Team standings </h4>
				</spa-link>
			</div>
			<div class="grid cols-3 fill-w margin-b-25 gap-25">
				<spa-link :target="'drivers/' + p.driver.id" :reqData="{ seasonId: season.id}" class="card flex flex-vertical padding-10 plain" v-for="(p, i) in profiles" :key="p.driver.id">
					<div class="flex fill-w align-center margin-b-5">
						<h3> #{{ i + 1 }} </h3>
						<div class="flex-grow text-end font-ml">
							Points: <b>{{ p.seasonPoints }}</b>
						</div>
					</div>
					<h2>
						{{ p.driver.nickname }}
					</h2>
					<div class="flex-grow relative fill-w height-250">
						<div class="absolute fill-h fill-w">
							<img class="object-position-bottom-right object-fit-contain fill-w fill-h"
								:src="p.profile.imageLink" />
						</div>
						<span class="absolute bottom-left">
							<h1 class="font-xl" :style="p.mostCommonTeam ? 'color: #' + p.mostCommonTeam.mainColor + '; text-shadow: 5px 5px #' + p.mostCommonTeam.secondaryColor : ''">{{ p.driver.number }}</h1>
						</span>
					</div>
				</spa-link>
			</div>
			<h1 class="font-l2 margin-b-25" v-if="chartOk">
				Points Timeline
			</h1>
			<canvas class="fill-w" id="driver-points-canvas">

			</canvas>
		</div>
	</base-page>
</template>

<script lang="ts">
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
		SpaLink
		
	}
})
export default class extends Vue{
	@Prop({default: (): DriverInfo[] => null}) readonly profiles: DriverInfo[];
	@Prop({default: (): Season => null}) readonly season: Season;

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