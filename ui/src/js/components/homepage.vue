<template>
	<base-page>
		<div class="grid sections-container">
			<my-section class="section-main padding-50 flex flex-vertical">
				<div class="font-l">
					Last race:
				</div>
				<div class="font-xxl">
					<span class="flag xxl spain">Spain SGP</span>
				</div>
				<!--<div>
					Watch the VOD <a class="twitch-link" href="#">Here! </a>
				</div>-->
				<span class="flex-grow" />
				<div class="font-l">
					Results:
				</div>
				<div class="font-l2">
					<team-banner team="renault"> JStefanelli </team-banner>
					<team-banner team="rp" > Clu </team-banner>
					<team-banner team="at" > Raycevick</team-banner>
				</div>
			</my-section>
			<my-section class="section-top-right flex flex-vertical padding-25">
				<div class="font-l">
					Next race:
				</div>
				<span class="flex-grow" />
				<div class="font-l" v-if="!nextRace || nextRace.isF2()">
					<i class="f2-first">SUPER</i> WEENIE HUT <i class="f2-second">SHAMBLES</i> {{(nextRace ? nextRace.capitalCountry() : "Monaco")}} GP
				</div>
				<div class="font-l" v-else>
					{{nextRace ? nextRace.toString() : "Some GP" }}
				</div>
				<div class="font-l">

				</div>
				<div class="font-xl" id="countdown" ref="countdown">
					12:00:00
				</div>
				<span class="flex-grow" />
				<div v-if="!nextRace || nextRace.isF2()">
					Live on <a class="twitch-link" href="https://twitch.tv/hammiam">Hammiam's channel</a>
				</div>
				<div v-else>
					Live on <a class="twitch-link" href="https://twitch.tv/LHudsonxx">LHudson's channel</a>
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
import Section from './homepage/section.vue';
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
	@Prop({default: null}) readonly nextRace : Race;
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