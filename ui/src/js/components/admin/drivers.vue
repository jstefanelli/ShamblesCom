<template>
	<base-layout v-bind="index">
		<div class="fill card no-hover padding-25 flex flex-vertical">
			<div class="flex align-center">
				<button class="passive" @click="goBack">
					Back
				</button>
				<span class="font-m margin-l-5">
					Drivers:
				</span>
			</div>
			<div class="flex-grow fill-h overflow-y-auto margin-v-5">
				<table class="fill-w">
					<thead>
						<tr>
							<th class="width-100">
								#
							</th>
							<th>
								Nickname
							</th>
							<th class="width-100">
								Number
							</th>
							<th class="width-100">
								Points
							</th>
							<th class="width-200">
								Edit
							</th>
						</tr>
					</thead>
					<tbody>
						<tr v-for="info in drivers" :key="info.driver.id">
							<td>
								{{ info.seasonPosition }}
							</td>
							<td>
								{{ info.driver.nickname }}
							</td>
							<td>
								{{ info.driver.number }}
							</td>
							<td>
								{{ info.seasonPoints }}
							</td>
							<td>
								<button @click="editDriver(info)">
									Edit
								</button>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
			<div class="flex">
				<div class="flex-grow" />
				<button class="passive" @click="createDriver">
					Create new Driver
				</button>
			</div>
		</div>
		<edit-driver-modal ref="driver-modal" :driver="selectedDriver" :season="season" />
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

@Component({
	components: {
		BaseLayout,
		EditDriverModal
	}
})
export default class extends Vue {
	@Prop({default: (): any => {}}) private index: any;
	@Prop({default: (): Season => null}) private season: Season;
	@Prop({default: (): DriverInfo[] => []}) private drivers: DriverInfo[];

	private selectedDriver: DriverInfo = null;


	private goBack() {
		console.log(this.season);
		SPA.navigateAndRender("/admin/" + this.season.id);
	}

	private createDriver() {
		this.selectedDriver = null;
		(this.$refs["driver-modal"] as any).open();
	}
}

</script>