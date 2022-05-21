<template>
	<base-page>
		<div class="flex flex-vertical gap-50 padding-v-50">
			<my-section class="section-main padding-50 flex flex-vertical">
				<h2 class="font-semibold">
					Last race:
				</h2>
				<div class="font-xxl">
					<spa-link v-if="lastRace" :target="'/race/' + lastRace.id" class="color-accent"><h1>{{ lastRace.name }}</h1></spa-link>
					<span class="xxl" v-else>No Data</span>
				</div>
				<h2 class="font-semibold">
					Results:
				</h2>
				<div class="height-350 fill-w grid cols-3 gap-25 font-m">
					<span class="font-xl text-border-5 flex-grow" v-if="!firstResult">
						No results available
					</span>

					<driver-card :result="firstResult" title="#1" />
					<driver-card :result="secondResult" title="#2" />
					<driver-card :result="thirdResult" title="#3" />
				</div>
				<div class="grid cols-2" style="grid-auto-rows: 1fr">
					<spa-link class="plain flex padding-5 align-center max-height-50" v-for="(rr, i) in otherResults" :key="rr.id" :target="'/driver/' + rr.driver.id">
						<h4>{{ rr.finished ? i + 4 : 'DNF' }}</h4>
						<h3 class="flex-grow text-end">{{ rr.driver.nickname }}</h3>
						<team-banner class="fill-h padding-v-5"
							:mainColor="rr.team.mainColor"
							:secondaryColor="rr.team.secondaryColor" >
						</team-banner>
					</spa-link>

				</div>
			</my-section>
			<div class="flex gap-50">
				<my-section class="section-top-right flex flex-vertical padding-25 flex-grow">
					<h2 class="font-semibold">
						Next up:
					</h2>
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
				<my-section class="section-bottom-right padding-25 flex-grow">
					<h2 class="font-semibold">
						Social:
					</h2>
					<div>
						<a class="twitter-link" href="https://twitter.com/shambleschamp"> Official Shambles Championship Twitter </a><br/>
						<a class="youtube-link" href="https://www.youtube.com/channel/UCwByts4CkdG2Oc_LO77wD3w"> Official Shambles Championship Clips channel</a><br/>
						<a class="twitch-link" href="https://twitch.tv/lhudsonx"> LHudson's Twitch Page </a><br/>
						<a class="twitch-link" href="https://twitch.tv/thepatyman"> ThePatyMan's Twitch Page </a><br/>
						<a class="twitch-link" href="https://twitch.tv/clu_snake"> Clu's Twitch Page </a><br/>
					</div>
					
				</my-section>
			</div>
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
import SpaLink from '@/SPA/SpaLink.vue';
import DriverCard from './layout/controls/driverCard.vue';

@Component({
	components: {
		BasePage,
		"my-section": Section,
		TeamBanner,
		"spa-link": SpaLink,
		DriverCard
	}
})
export default class extends Vue {
	@Prop({default: (): Race => null}) readonly lastRace : Race;
	@Prop({default: (): Race => null}) readonly nextRace : Race;

	private timerThread: number = 0;

	public mounted() {

		this.updateTimer();
		this.timerThread = window.setInterval(() => {
			this.updateTimer();

		}, 1000);
	}

	public get firstResult() : RaceResult {
		if (this.lastRace && this.lastRace.raceResults != null && this.lastRace.raceResults.length > 0) {
			return this.lastRace.raceResults[0];
		}
		return null;
	}

	public get secondResult() : RaceResult {
		if (this.lastRace && this.lastRace.raceResults != null && this.lastRace.raceResults.length > 1) {
			return this.lastRace.raceResults[1];
		}
		return null;
	}
	
	public get thirdResult() : RaceResult {
		if (this.lastRace && this.lastRace.raceResults != null && this.lastRace.raceResults.length > 2) {
			return this.lastRace.raceResults[2];
		}
		return null;
	}

	public get otherResults() : RaceResult[] {
		if (this.lastRace && this.lastRace.raceResults != null && this.lastRace.raceResults.length >= 4) {
			return this.lastRace.raceResults.slice(3);
		}

		return [];
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