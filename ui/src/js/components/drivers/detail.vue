<template>
	<base-page>
		<div class="flex flex-col w-full h-full gap-5 p-5">
			<div class="flex gap-2 items-baseline">
				<span class="text-4xl font-bold">
					{{ profile.driver.nickname }}
				</span>
				<span class="text-lg">
					{{ profile.shortDescription }}
				</span>
			</div>
			<div class="flex flex-grow gap-5">
				<div class="flex-[2] flex flex-col gap-3 justify-evenly">
					<card class="flex-1 relative p-3">
						<img class="absolute inset-0 w-full h-full object-contain object-right-bottom" :src="profile.imageLink">
						<span v-if="profile" class="absolute top-3 left-3 font-f1 text-6xl font-bold" 
							:style="'color: #' + (mainTeam ? mainTeam.mainColor : 'FFF' ) + '; text-shadow: 5px 5px #' + (mainTeam ? mainTeam.secondaryColor : '000')">
							{{ profile.driver.number }}
						</span>
					</card>
					<card class="flex-1 flex flex-col gap-3 p-3">
						<span class="text-2xl font-bold">
							Info
						</span>
						<span class="text-lg">
							<b>Number:</b> {{ profile.driver.number }}
						</span>
						<span class="text-lg">
							<b>Points:</b> {{ seasonPoints }}
						</span>
						<div class="flex gap-2">
							<span class="text-lg" v-if="mainTeam">
								<b>Team:</b> 
							</span>
							<team-banner :mainColor="mainTeam.mainColor" :secondaryColor="mainTeam.secondaryColor">{{ mainTeam.name }}</team-banner>
						</div>
						<span class="text-lg" v-if="bestSeason">
							<b>Season best finish: </b> {{ bestSeason.position}} at <spa-link class="text-red-titlebar font-f1" :target="'/race/' + bestSeason.raceId">{{ bestSeason.race.name }}</spa-link>
						</span>
						<span class="text-lg">
							<b>Bio:</b> <br>
							{{ profile.description }}
						</span>
					</card>
				</div>
				<card class="flex flex-col flex-[3] gap-3 p-3">
					<span class="text-2xl font-bold">
						Results
					</span>
					<div class="flex-grow flex flex-col gap-3 overflow-y-auto">
						<div class="rounded-lg flex items-center gap-3 text-xl p-5 w-full border-2 border-white border-opacity-30" v-for="r in results" :key="r.id">
							<span>
								{{ r.finished ? r.position : 'DNF'}}
							</span>
							<team-banner class="align-self-stretch" :mainColor="r.team.mainColor" :secondaryColor="r.team.secondaryColor">
								{{ r.team.name }}
							</team-banner>
							<span class="font-f1 text-xl text-center font-bold text-red-titlebar flex-grow">
								{{ r.race.name }}
							</span>
							<span>
								{{ r.points }} points
							</span>
						</div>
					</div>
				</card>
			</div>
		</div>
	</base-page>
</template>

<script lang="ts">
import BasePage from '../layout/basePage.vue';
import Section from '../layout/section.vue';
import { Vue, Component, Prop } from 'vue-property-decorator';
import Season from '@/data/Season';
import DriverProfile from '@/data/DriverProfile';
import TeamBanner from '../layout/teamBanner.vue';
import RaceResult from '@/data/RaceResult';
import Team from '@/data/Team';
import SpaLink from '@/SPA/SpaLink.vue';

const ResultsPerPage = 10;

@Component({
	components: {
		BasePage,
		'card': Section,
		TeamBanner,
		SpaLink
	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) public readonly season: Season;
	@Prop({default: (): DriverProfile => null}) public readonly profile: DriverProfile;
	@Prop({default: (): Team => null}) public readonly mainTeam: Team;
	@Prop({default: 0}) public readonly totalEvents: number;
	@Prop({default: 0}) public readonly seasonEvents: number;
	@Prop({default: (): RaceResult => null}) public readonly bestOverall: RaceResult;
	@Prop({default: (): RaceResult => null}) public readonly bestSeason: RaceResult;
	@Prop({default: 0}) public readonly seasonPoints: number;

	public results: RaceResult[] = null;
	public resultsOffset: number = 0;
	public moreResultsAvailable: boolean = true;

	public mounted() {
		this.updateResults();
	}

	public requestResults(offset: number) {
		return new Promise<RaceResult[]>((resolve, fail) => {
			const url = "/api/driver/" + this.profile.driverId + "/Results?skip=" + (this.resultsOffset * ResultsPerPage) + "&count=" + ResultsPerPage;
			fetch(url).then((res) => {
				res.json().then(data => {
					if(Array.isArray(data) && data.length > 0) {
						resolve(data as RaceResult[]);
					} else {
						fail("Invalid response object or no results");
					}
				}).catch(err => {
					console.error("[Drivers/Detail] Failed to parse Result request: ", err);
					fail("Failed to parse result response");
				});
			}).catch(err => {
				console.error("[Drivers/Detail] Result request errored: ", err);
				fail("Server error");
			})
		})
	}

	public updateResults() {
		this.requestResults(this.resultsOffset).then(r => {
			if (this.results) {
				r.forEach(rr => this.results.push(rr));
			} else {
				this.results = r;
			}
			this.requestResults(this.resultsOffset + 1).then(() => {
				this.moreResultsAvailable = true;
			}).catch(() => {
				this.moreResultsAvailable = false;
			})
		}).catch(() => {
			this.results = null;
		});
	}

	public nextPage() {
		this.resultsOffset++;
		this.updateResults();
	}

	public prevPage() {
		if (this.resultsOffset == 0)
			return;

		this.resultsOffset--;
		this.updateResults();
	}

}

</script>