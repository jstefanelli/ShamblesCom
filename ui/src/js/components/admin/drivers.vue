<template>
	<base-layout v-bind="index">
		<div class="fill card no-hover padding-25">

		</div>
		<edit-driver-modal :driver="selectedDriver" />
	</base-layout>
</template>

<script lang="ts">
import Driver from '@/data/Driver';
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
	@Prop({default: (): Driver[] => []}) private drivers: Driver[];

	private selectedDriver: Driver = null;
}

</script>