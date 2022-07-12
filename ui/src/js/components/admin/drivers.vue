<template>
	<base-layout v-bind="index">
		<card class="w-full h-full p-6 flex flex-col">
			<div class="flex items-center">
				<Mybutton @click="goBack">
					Back
				</Mybutton>
				<span class="text-xl ml-1">
					Drivers:
				</span>
			</div>
			<div class="flex flex-grow h-full">
				<div class="flex-grow-2 flex flex-col m-1">
					<div class="text-xl">
						Season Drivers:
					</div>
					<table class="w-full">
						<thead>
							<tr>
								<th class="w-24">
									#
								</th>
								<th>
									Nickname
								</th>
								<th class="w-24">
									Number
								</th>
								<th class="w-24">
									Points
								</th>
								<th class="w-48">
									Edit Nickname/Number
								</th>
								<th class="w-48">
									Edit Profile
								</th>
							</tr>
						</thead>
						<tbody class="overflow-y-auto p-1">
							<tr class="border-b-2 border-b-white border-opacity-50" v-for="info in drivers" :key="info.driver.id">
								<td class="text-center border-r-2 border-r-white border-opacity-50 px-1">
									{{ info.seasonPosition }}
								</td>
								<td class="text-center border-r-2 border-r-white border-opacity-50 px-1">
									{{ info.driver.nickname }}
								</td>
								<td class="text-center border-r-2 border-r-white border-opacity-50 px-1">
									{{ info.driver.number }}
								</td>
								<td class="text-center border-r-2 border-r-white border-opacity-50 px-1">
									{{ info.seasonPoints }}
								</td>
								<td class="text-center border-r-2 border-r-white border-opacity-50 px-1">
									<Mybutton @click="editDriver(info.driver)">
										Edit Info
									</Mybutton>
								</td >
								<td class="text-center px-1">
									<Mybutton :main="true" @click="navigate('/admin/' + index.selectedSeasonId + '/drivers/' + info.driver.id)">
										Edit Profile
									</Mybutton>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
				<div class="flex-grow flex flex-col m-1">
					<div class="text-xl">
						Other Drivers:
					</div>
					<table class="w-full">
						<thead>
							<tr>
								<th>
									Nickname
								</th>
								<th class="w-24">
									Number
								</th>
								<th>
									Edit
								</th>
							</tr>
						</thead>
						<tbody class="overflow-y-auto p-1">
							<tr class="border-b-2 border-b-white border-opacity-50" v-for="d in other_drivers" :key="d.id">
								<td class="text-center border-r-2 border-r-white border-opacity-50 px-1">
									{{ d.nickname }}
								</td>
								<td class="text-center border-r-2 border-r-white border-opacity-50 px-1">
									{{ d.number }}
								</td>
								<td class="text-center px-1">
									<Mybutton :main="false" @click="editDriver(d)">
										Edit
									</Mybutton>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="flex">
				<div class="flex-grow" ></div>
				<Mybutton @click="createDriver">
					Create new Driver
				</Mybutton>
			</div>
		</card>
		<edit-driver-modal ref="driver-modal" :driver="selectedDriver" :season="season" ></edit-driver-modal>
	</base-layout>
</template>

<script lang="ts">
import Driver from '@/data/Driver';
import DriverInfo from '@/data/DriverInfo';
import Season from '@/data/Season';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import BaseLayout from './baseLayout.vue';
import EditDriverModal from './modals/editDriverModal.vue';
import Section from '../layout/section.vue';
import Mybutton from '../layout/controls/mybutton.vue';

@Component({
	components: {
		BaseLayout,
		EditDriverModal,
		"card": Section,
		Mybutton
	}
})
export default class extends Vue {
	@Prop({default: (): any => {}}) public index: any;
	@Prop({default: (): Season => null}) public season: Season;
	@Prop({default: (): DriverInfo[] => []}) public drivers: DriverInfo[];
	@Prop({default: (): Driver[] => []}) public other_drivers: Driver[];

	public selectedDriver: Driver = null;

	public navigate(target: string) {
		SPA.navigateAndRender(target);
	}

	public goBack() {
		console.log(this.season);
		SPA.navigateAndRender("/admin/" + this.season.id);
	}

	public createDriver() {
		this.selectedDriver = null;
		(this.$refs["driver-modal"] as any).open();
	}

	public editDriver(d: Driver) {
		this.selectedDriver = d;
		(this.$refs["driver-modal"] as any).open();
	}
}

</script>