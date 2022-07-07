<template>
	<card>
		<spa-link :target="'/drivers/' + result.driver.id" class="flex flex-col h-full p-1 text-white" v-if="result">
			<div class="flex-grow w-full relative">
				<span class=" text-8xl font-semibold text-border-5 absolute"
					:style="'color: #' + result.team.mainColor + '; text-shadow: 10px 10px #' + result.team.secondaryColor">
					{{ title }}
				</span>
				<div class="absolute w-full h-full">
					<img class=" object-right-bottom object-contain w-full h-full"
						:src="imageLink"
					>
				</div>
			</div>
			<team-banner
				:mainColor="result.team.mainColor"
				:secondaryColor="result.team.secondaryColor"> 
				{{ result.driver.nickname }}
			</team-banner>
		</spa-link>
	</card>
</template>

<script lang="ts">
import Section from '../section.vue';
import RaceResult from '@/data/RaceResult';
import SpaLink from '@/SPA/SpaLink.vue';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import TeamBanner from '../teamBanner.vue';
import MissingDriver from "£/images/Missing_profile.png"

@Component({
	components: {
		SpaLink,
		TeamBanner,
		"card": Section
	}
})
export default class extends Vue {
	@Prop({default: () : RaceResult => null}) readonly result: RaceResult;
	@Prop({default: "#1"}) readonly title: string;

	public get imageLink(): string {
		let res = this.result;
		let link = MissingDriver;
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
}
</script>