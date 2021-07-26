<template>
	<base-page>
		<div class="grid sections-container">
			<my-section class="section-main padding-50 flex flex-vertical">
				<div class="font-l">
					Last race:
				</div>
				<div class="font-xxl">
					<span class="xxl">{{ lastRace ? lastRace.name : "No Data" }} </span>
				</div>
				<!--<div>
					Watch the VOD <a class="twitch-link" href="#">Here! </a>
				</div>-->
				<span class="flex-grow" />
				<div class="font-l">
					Results:
				</div>
				<div class="font-l2">
					<team-banner v-if="lastRace != null && lastRace.raceResults != null && (lastRace.raceResults.length > 0)"
						:mainColor="lastRace.raceResults[0].team.mainColor"
						:secondaryColor="lastRace.raceResults[0].team.secondaryColor"> 
						{{ lastRace.raceResults[0].driver.nickname }}
					</team-banner>
					
					<team-banner v-if="lastRace && lastRace.raceResults && (lastRace.raceResults.length > 1)"
						:mainColor="lastRace.raceResults[1].team.mainColor"
						:secondaryColor="lastRace.raceResults[1].team.secondaryColor"> 
						{{ lastRace.raceResults[1].driver.nickname }}
					</team-banner>
					
					<team-banner v-if="lastRace && lastRace.raceResults && lastRace.raceResults.length > 2"
						:mainColor="lastRace.raceResults[2].team.mainColor"
						:secondaryColor="lastRace.raceResults[2].team.secondaryColor"> 
						{{ lastRace.raceResults[2].driver.nickname }}
					</team-banner>
				</div>
			</my-section>
			<my-section class="section-top-right flex flex-vertical padding-25">
				<div class="font-l">
					Next race:
				</div>
				<div class="font-l">
					{{ nextRace ? nextRace.name : "No Data" }}
				</div>
				<div class="font-l">

				</div>
				<div class="font-xl" id="countdown" ref="countdown">
					No Data
				</div>
				<span class="flex-grow" />
				<div>
					Live <a class="twitch-link" :href="nextRace ? nextRace.livestreamLink : 'https://twitch.tv/lhudsonx'">Here</a>
				</div>
			</my-section>
			<my-section class="section-bottom-right padding-25">
				<div class="font-l">
					Social:
				</div>
				<div>
					<a class="twitter-link" href="#"> Official Shambles Championship Twitter </a><br/>
					<a class="youtube-link" href="#"> Official Shambles Championship Clips channel</a><br/>
					<a class="twitch-link" href="#"> LHudson's Twitch Page </a><br/>
					<a class="twitch-link" href="#"> ThePatyMan's Twitch Page </a><br/>
					<a class="twitch-link" href="#"> Clu's Twitch Page </a><br/>
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

@Component({
	components: {
		BasePage,
		"my-section": Section,
		TeamBanner
	}
})
export default class extends Vue {
	@Prop({default: (): Race => null}) readonly lastRace : Race;
	@Prop({default: (): any => {}}) private nextRace : Race;

	private timerThread: number = 0;

	public mounted() {
		console.log("Next: ", this.nextRace);

		this.updateTimer();
		this.timerThread = window.setInterval(() => {
			if (this.nextRace ) {
				this.updateTimer();		
			}

		}, 1000);
	}

	private updateTimer() {
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
</style>