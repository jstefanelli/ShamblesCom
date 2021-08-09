<template>
	<base-page>
		<div class="grid sections-container">
			<my-section class="section-main padding-50 flex flex-vertical">
				<div class="font-l">
					Last race:
				</div>
				<div class="font-xxl">
					<spa-link v-if="lastRace" :target="'/race/' + lastRace.id" class="color-accent">{{ lastRace.name }}</spa-link>
					<span class="xxl" v-else>No Data</span>
				</div>
				<div class="font-l">
					Results:
				</div>
				<div class="relative flex-grow">
					<div class="absolute relative-center fill-w fill-h">
						<div class="font-m images-container fill-w fill-h">
							<span class="rank-first font-xxxl text-border-5"
								:style="'color: #' + firstResult.team.mainColor + '; text-shadow: 10px 10px #' + firstResult.team.secondaryColor">
								#1
							</span>

							<img class="object-position-bottom-right object-fit-contain images-first"
								v-if="firstResult != null"
								:src="firstImageLink"
							/>
							<team-banner class="title-first" v-if="firstResult != null"
								:mainColor="firstResult.team.mainColor"
								:secondaryColor="firstResult.team.secondaryColor"> 
								{{ firstResult.driver.nickname }}
							</team-banner>

							<span class="rank-second font-xxxl text-border-5"
								:style="'color: #' + secondResult.team.mainColor + '; text-shadow: 10px 10px #' + secondResult.team.secondaryColor">
								#2
							</span>

							<img class="object-position-bottom-right object-fit-contain images-second"
								v-if="secondResult != null"
								:src="secondImageLink"
							/>

							<team-banner class="title-second" v-if="secondResult"
								:mainColor="secondResult.team.mainColor"
								:secondaryColor="secondResult.team.secondaryColor"> 
								{{ secondResult.driver.nickname }}
							</team-banner>
							
							<span class="rank-third font-xxxl text-border-5"
								:style="'color: #' + thirdResult.team.mainColor + '; text-shadow: 10px 10px #' + thirdResult.team.secondaryColor">
								#3
							</span>

							<img class="object-position-bottom-right object-fit-contain images-third"
								v-if="thirdResult != null"
								:src="thirdImageLink"
							/>
							<team-banner class="title-third" v-if="thirdResult != null"
								:mainColor="thirdResult.team.mainColor"
								:secondaryColor="thirdResult.team.secondaryColor"> 
								{{ thirdResult.driver.nickname }}
							</team-banner>
						</div>
					</div>
				</div>
			</my-section>
			<my-section class="section-top-right flex flex-vertical padding-25">
				<div class="font-l">
					Next up:
				</div>
				<div class="font-l margin-v-10">
					<spa-link v-if="nextRace" :target="'/race/' + nextRace.id" class="color-accent"> {{ nextRace.name }}</spa-link>
					<span v-else>No race planned</span>
				</div>
				<div class="font-l">

				</div>
				<div class="font-xl" id="countdown" ref="countdown">
				</div>
				<span class="flex-grow" />
				<div v-if="nextRace && nextRace.livestreamLink != null && nextRace.livestreamLink.length > 0">
					Live <a class="twitch-link" :href="nextRace.livestreamLink">Here</a>
				</div>
			</my-section>
			<my-section class="section-bottom-right padding-25">
				<div class="font-l">
					Social:
				</div>
				<div>
					<a class="twitter-link" href="https://twitter.com/shambleschamp"> Official Shambles Championship Twitter </a><br/>
					<a class="youtube-link" href="https://www.youtube.com/channel/UCwByts4CkdG2Oc_LO77wD3w"> Official Shambles Championship Clips channel</a><br/>
					<a class="twitch-link" href="https://twitch.tv/lhudsonx"> LHudson's Twitch Page </a><br/>
					<a class="twitch-link" href="https://twitch.tv/thepatyman"> ThePatyMan's Twitch Page </a><br/>
					<a class="twitch-link" href="https://twitch.tv/clu_snake"> Clu's Twitch Page </a><br/>
				</div>
				
			</my-section>
		</div>
	</base-page>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import BasePage from './layout/basePage.vue';
import Section from './layout/section.vue';
import TeamBanner from './layout/teamBanner.vue';
import Race from '@/data/Race';
import RaceResult from '@/data/RaceResult';
import MissingDriver from "£/images/Missing_profile.png"
import SpaLink from '@/SPA/SpaLink.vue';

@Component({
	components: {
		BasePage,
		"my-section": Section,
		TeamBanner,
		"spa-link": SpaLink
	}
})
export default class extends Vue {
	@Prop({default: (): Race => null}) readonly lastRace : Race;
	@Prop({default: (): any => {}}) private nextRace : Race;

	private timerThread: number = 0;

	public mounted() {

		this.updateTimer();
		this.timerThread = window.setInterval(() => {
			this.updateTimer();

		}, 1000);
	}

	public get firstResult() : RaceResult {
		if (this.lastRace.raceResults != null && this.lastRace.raceResults.length > 0) {
			return this.lastRace.raceResults[0];
		}
		return null;
	}
	
	public get firstImageLink() : string {
		let link: string = MissingDriver;
		let res = this.firstResult;
		if (res != null &&
			res.driver != null &&
			res.driver.profiles != null &&
			res.driver.profiles.length > 0 &&
			res.driver.profiles[0] != null &&
			res.driver.profiles[0].imageLink != null &&
			res.driver.profiles[0].imageLink.length > 0) {
			link = res.driver.profiles[0].imageLink;
		}
		return link;
	}
	
	public get secondResult() : RaceResult {
		if (this.lastRace.raceResults != null && this.lastRace.raceResults.length > 1) {
			return this.lastRace.raceResults[1];
		}
		return null;
	}

	public get secondImageLink() : string {
		let link: string = MissingDriver;
		let res = this.secondResult;
		if (res != null &&
			res.driver != null &&
			res.driver.profiles != null &&
			res.driver.profiles.length > 0 &&
			res.driver.profiles[0] != null &&
			res.driver.profiles[0].imageLink != null &&
			res.driver.profiles[0].imageLink.length > 0) {
			link = res.driver.profiles[0].imageLink;
		}
		return link;
	}
	
	public get thirdResult() : RaceResult {
		if (this.lastRace.raceResults != null && this.lastRace.raceResults.length > 2) {
			return this.lastRace.raceResults[2];
		}
		return null;
	}

	public get thirdImageLink() : string {
		let link: string = MissingDriver;
		let res = this.thirdResult;
		if (res != null &&
			res.driver != null &&
			res.driver.profiles != null &&
			res.driver.profiles.length > 0 &&
			res.driver.profiles[0] != null &&
			res.driver.profiles[0].imageLink != null &&
			res.driver.profiles[0].imageLink.length > 0) {
			link = res.driver.profiles[0].imageLink;
		}
		return link;
	}

	private updateTimer() {
		if (!this.nextRace)
			return;

		let now = new Date();
		let raceDate = new Date(this.nextRace.dateTime);

		var amount = raceDate.getTime() - now.getTime();
		let hours = Math.floor(amount / (1000 * 60 * 60));
		amount -= hours * 1000 * 60 * 60;
		let minutes = Math.floor(amount / (1000 * 60));
		amount -= minutes * 1000 * 60;
		let seconds = Math.floor(amount / 1000);

		let str = "" + (hours < 10 ? "0" + hours : hours) + ":" + (minutes < 10 ? "0" + minutes : minutes) + ":" + (seconds < 10 ? "0" + seconds : seconds);
		document.getElementById("countdown").innerText = str;
	}

	public unmounted() {
		window.window.clearInterval(this.timerThread);
	}
}

</script>

<style lang="scss">
.sections-container {
	grid-template-columns: 2fr minmax(250px, 1fr);
	grid-template-rows: 1fr 1fr;
	grid-gap: 50px;
	padding: 50px;
	height: calc(100vh - 100px);
}

.section-main {
	grid-column: 1;
	grid-row-start: 1;
	grid-row-end: 3;
}

.section-top-right {
	grid-column: 2;
	grid-row: 1;
}

.section-top-left {
	grid-column: 2;
	grid-row: 2;
}

.images-container {
	display: grid;
	grid-template-columns: 40% 30% 30%;
	grid-template-rows: 20% minmax(0, 1fr) auto;
	max-height: 100%;
}

.images-first {
	width: 100%;
	height: 100%;
	grid-row-start: 1;
	grid-row-end: 3;
	grid-column: 1;
}

.rank-first {
	width: 100%;
	height: 100%;
	grid-row-start: 1;
	grid-row-end: 3;
	grid-column: 1;
}

.images-second {
	width: 100%;
	height: 100%;
	grid-row-start: 2;
	grid-row-end: 3;
	grid-column: 2;
}

.rank-second {
	width: 100%;
	height: 100%;
	grid-row-start: 1;
	grid-row-end: 3;
	grid-column: 2;
}

.images-third {
	width: 100%;
	height: 100%;
	grid-row-start: 2;
	grid-row-end: 3;
	grid-column: 3;
}

.rank-third {
	width: 100%;
	height: 100%;
	grid-row-start: 1;
	grid-row-end: 3;
	grid-column: 3;
}

.title-first {
	grid-row: 3;
	grid-column: 1;
}

.title-second {
	grid-row: 3;
	grid-column: 2;
}

.title-third {
	grid-row: 3;
	grid-column: 3;
}
</style>