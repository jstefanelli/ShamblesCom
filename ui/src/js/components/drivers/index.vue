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
				<spa-link class="card flex flex-vertical padding-10 plain" v-for="(p, i) in profiles" :key="p.driver.id">
					<div class="flex fill-w align-center margin-b-5">
						<h3> {{ i + 1 }} </h3>
						<div class="flex-grow text-end">
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
		</div>
	</base-page>
</template>

<script lang="ts">
import DriverInfo from '@/data/DriverInfo';
import Season from '@/data/Season';
import SpaLink from '@/SPA/SpaLink.vue';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import BasePage from '../layout/basePage.vue';


@Component({
	components: {
		BasePage,
		SpaLink
		
	}
})
export default class extends Vue{
	@Prop({default: (): DriverInfo[] => null}) readonly profiles: DriverInfo[];
	@Prop({default: (): Season => null}) readonly season: Season;
}
</script>