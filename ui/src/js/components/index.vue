<template>
	<div class="h-screen index-background">
		<spa-link target="/homepage" class=" h-full w-full flex flex-col items-center">
			<div class=" flex-grow" ></div>
			<div :style="'height: ' + (targetBarHeight - currentBarHeight) + 'px'" ></div>
			<div class="index-logo overflow-hidden">
				<img :src="require('£/images/logo_borderless.png')">
			</div>
			<dov class=" h-[25px]" ></dov>
			<div class="index-continue flex">
				<div class="flex-grow bg-white" ></div>
				<svg :viewBox="currentViewBox()" width="525" :height="currentBarHeight">
					<mask id="msk0">
						<rect x="0" y="0" width="10000" height="100" fill="white" ></rect>
						<text x="5" :y="currentTextOffset" style="font-weight: bold" fill="black">CLICK TO CONTINUE </text>
					</mask>
					<rect x="0" y="0" width="10000" height="100" fill="white" mask="url(#msk0)" ></rect>
				</svg>
				<div class="flex-grow bg-white" ></div>
			</div>
			<div class="flex-grow" ></div>
		</spa-link>
	</div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import SpaLink from '@/SPA/SpaLink.vue';

@Component({
	components: {
		SpaLink
	}
})
export default class extends Vue {
	private lerp(low: number, high: number, val: number) {
		return low + ((high - low) * val);
	}

	private easeIn(startTime: number, endTime: number, currentTime: number) {
		let timeLen = endTime - startTime;
		let x = Math.max(0, Math.min(1, (currentTime - startTime) / timeLen));

		return Math.sin((x * Math.PI) / 2);
	}

	animate(tm: number): void {
		if (this.lastTimestamp == 0) {
			this.lastTimestamp = tm;
		} else {

			console.log(this.animationStatus);
			switch (this.animationStatus) {
				case -1:
				{
					if (this.currentWaitTimer == 0) {
						this.currentWaitTimer = tm;
					}

					if ((tm - this.currentWaitTimer) > this.targetWaitTime) {
						this.animationStatus++;
					}
				}
				break;
				case 0:
				{
					if (this.animStartTime == 0) {
						this.animStartTime = tm;
					}

					let x = this.easeIn(this.animStartTime, this.animStartTime + this.targetTime1, tm);
					this.currentBarHeight = this.lerp(0, this.targetBarHeight, x);

					if (x >= 1) {
						this.animationStatus++;
					}
				}
				break;
				case 1:
				{
					if (this.anim2StartTime == 0) {
						this.anim2StartTime = tm;
					}

					let x = this.easeIn(this.anim2StartTime, this.anim2StartTime + this.targetTime2, tm);
					this.currentTextOffset = this.lerp(70, this.targetTextOffset, x);
					if (x >= 1) {
						return;
					}
				}
				break;
			}
		}

		window.requestAnimationFrame((tm) => this.animate(tm));
	}

	public currentWaitTimer: number = 0;
	public animationStatus: number = -1;
	public lastTimestamp: number = 0;
	public animStartTime: number = 0;
	public anim2StartTime: number = 0;
	public currentBarHeight: number = 0;
	public currentViewBox() : string {
		return "0 0 525 " + this.currentBarHeight;
	}

	public currentTextOffset: number = 70;

	public readonly targetWaitTime: number = 250;
	public readonly targetTime1: number = 750;
	public readonly targetTime2: number = 500;
	public readonly targetTextOffset: number = 34;
	public readonly targetBarHeight: number = 43;

	mounted() {
		this.animationStatus = -1;
		window.requestAnimationFrame((tm) => this.animate(tm));
	}
}

</script>

<style lang="scss">

.index-background {
	background-image: url('£/images/grid.png');
	background-size: cover;
	background-position: center;
	background-repeat: no-repeat;
}

.index-logo {
	width: 80vh;
	height: auto;

	> img {
		width: 100%;
		height: auto;
	}
}

.index-continue {
	width: 80vh;
	height: auto;

	> svg {
		font-family: 'Neue Haas Display';
		font-style: normal;
		font-weight: black;
		letter-spacing: 8pt;
		font-size: 25pt;
	}
}
</style>