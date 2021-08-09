<template>
	<base-page>
		<div class="flex flex-vertical fill page-content padding-25">
			<div class="fill-w font-l margin-b-10">
				{{ season ? season.name : "Unknown season"}} standings:
			</div>
			<div class="flex fill-w flex-grow">
				<div class="margin-r-25 padding-5 flex flex-vertical width-350 card no-hover " v-if="firstDriver">
					<span class="font-m">
						Drivers Championship Leader:
					</span>
					<spa-link :target="'/driver/' + firstDriver.profile.id" class="font-l2">
						{{ firstDriver.driver.nickname }}
					</spa-link>
					<img class="object-position-center object-fit-contain"
						:src="leaderImageLink" />
					<span class="font-l">

					</span>
					<span class="font-l">
						Points: {{ firstDriver.seasonPoints }}
					</span>
					<span class="font-l">
						Wins: {{ leaderWins }}
					</span>
					<span class="font-l">
						Podiums: {{ leaderPodiums }}
					</span>
					<span class="font-l">
						Pole Positions: {{ leaderPoles }}
					</span>
				</div>
				<div class="flex-grow card no-hover padding-5">
					<span class="font-m">
						Drivers Championship standings:
					</span>
					<table class="fill-w flex-grow font-m">
						<thead class="font-haas italics">
							<th class="width-100">
								#
							</th>
							<th>
								Driver
							</th>
							<th class="width-200">
								Points
							</th>
							<th class="width-200">
								Wins
							</th>
						</thead>
						<tbody>
							<tr v-for="driver in drivers" :key="driver.driver.id">
								<td>
									{{ driver.seasonPosition }}
								</td>
								<td>
									<spa-link :target="'/driver/' + driver.profile.id">
										{{ driver.driver.nickname }}
									</spa-link>
								</td>
								<td>
									{{ driver.seasonPoints }}
								</td>
								<td>
									{{  driverWins(driver) }}
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</div>
	</base-page>
</template>

<script lang="ts">
import Driver from '@/data/Driver';
import DriverInfo from '@/data/DriverInfo';
import Season from '@/data/Season';
import SpaLink from '@/SPA/SpaLink.vue';
import { Vue, Prop, Watch, Component } from 'vue-property-decorator';
import BasePage from '../layout/basePage.vue';
import MissingProfileLink from '£/images/Missing_profile.png';

@Component({
	components: {
		BasePage,
SpaLink
		
	}
})
export default class extends Vue {
	@Prop({default: ():any => null}) private readonly season: Season;
	@Prop({default: ():any => []}) private readonly drivers: DriverInfo[];

	public get firstDriver(): DriverInfo {
		if (this.drivers == null || this.drivers.length == 0)
			return null;
		return this.drivers[0];
	}

	private get leaderImageLink(): string {
		let drv = this.firstDriver;
		if (drv == null || drv.profile == null)
			return MissingProfileLink;
		return drv.profile.imageLink;
	}

	public get leaderWins(): number {
		return this.driverWins(this.firstDriver);
	}

	public get leaderPodiums(): number {
		let drv = this.firstDriver;
		if (drv == null || drv.driver == null || drv.driver.raceResults == null)
			return 0;
		let pods = 0;
		drv.driver.raceResults.forEach((rr) => {
			if (rr.position < 4 && rr.finished) {
				pods += 1;
			}
		});

		return pods;
	}
	public get leaderPoles(): number {
		let drv = this.firstDriver;
		if (drv == null || drv.driver == null || drv.driver.raceResults == null)
			return 0;
		let poles = 0;
		drv.driver.raceResults.forEach((rr) => {
			if (rr.startPosition == 1) {
				poles += 1;
			}
		});

		return poles;
	}

	public driverWins(drv: DriverInfo): number {
		if (drv == undefined || drv == null || drv.driver == null || drv.driver.raceResults == null)
			return 0;
		let wins = 0;
		drv.driver.raceResults.forEach((rr) => {
			if (rr.position == 1 && rr.finished) {
				wins += 1;
			}
		});

		return wins;
	}
}
</script>