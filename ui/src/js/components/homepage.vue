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
				<div class="font-l">
					Results:
				</div>
				<div class="relative flex-grow">
					<div class="absolute relative-center fill-w fill-h">
						<div class="font-m images-container fill-w fill-h">
							<img class="object-position-bottom object-fit-contain images-first"
								:src="lastRace.raceResults[0].driver.profiles[0].imageLink"
							/>
							<team-banner class="title-first"
								:mainColor="lastRace.raceResults[0].team.mainColor"
								:secondaryColor="lastRace.raceResults[0].team.secondaryColor"> 
								{{ lastRace.raceResults[0].driver.nickname }}
							</team-banner>

							<img class="object-position-bottom object-fit-contain images-second"
								:src="lastRace.raceResults[1].driver.profiles[0].imageLink"
							/>
							<team-banner class="title-second"
								:mainColor="lastRace.raceResults[1].team.mainColor"
								:secondaryColor="lastRace.raceResults[1].team.secondaryColor"> 
								{{ lastRace.raceResults[1].driver.nickname }}
							</team-banner>

							<img class="object-position-bottom object-fit-contain images-third"
								:src="lastRace.raceResults[2].driver.profiles[0].imageLink"
							/>
							<team-banner class="title-third"
								:mainColor="lastRace.raceResults[2].team.mainColor"
								:secondaryColor="lastRace.raceResults[2].team.secondaryColor"> 
								{{ lastRace.raceResults[2].driver.nickname }}
							</team-banner>
						</div>
					</div>
				</div>
			</my-section>
			<my-section class="section-top-right flex flex-vertical padding-25">
				<div class="font-l">
					Next up:
				</div>
				<div class="font-l">
					{{ nextRace ? nextRace.name : "No race planned" }}
				</div>
				<div class="font-l">

				</div>
				<div class="font-xl" id="countdown" ref="countdown">
				</div>
				<span class="flex-grow" />
				<div v-if="nextRace">
					Live <a class="twitch-link" :href="nextRace.livestreamLink">Here</a>
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

	private showImage(n: number) {
		return this.lastRace != null && this.lastRace.raceResults != null && this.lastRace.raceResults.length > n - 1;
	}

	public mounted() {
		console.log("Last: ", this.lastRace.raceResults[0].driver);

		this.updateTimer();
		this.timerThread = window.setInterval(() => {
			this.updateTimer();

		}, 1000);
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

.images-second {
	width: 100%;
	height: 100%;
	grid-row-start: 2;
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