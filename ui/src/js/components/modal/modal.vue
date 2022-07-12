<template>
	<transition name="fade">
		<div class="modal fixed overflow-x-hidden overflow-y-auto inset-0 top-28 bg-opacity-50 bg-black " v-if="visible">
			<div class="flex items-start">
				<div class="flex-grow" ></div>
				<card class="flex flex-col mt-6 p-6 bg-opacity-100">
					<div class="text-3xl  mb-6">
						<slot name="header" ></slot>
					</div>

					<div class="flex-grow mb-6">
						<slot ></slot>
					</div>

					<div class="ml-px flex items-center gap-2">
						<slot name="footer-heading" ></slot>
						<div class="flex-grow" ></div>
						<slot name="footer" ></slot>
					</div>
				</card>
				<div class="flex-grow" ></div>
			</div>
		</div>
	</transition>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator';
import Section from '@/components/layout/section.vue';

@Component({
	components: {
		"card": Section
	}
})
export default class extends Vue {
	visible: boolean = false;

	public closeModal() {
		this.visible = false;
		this.$emit("closed");
	}

	public openModal() {
		this.visible = true;
		this.$emit("opened");
	}
}
</script>

<style lang="scss">
.modal {

	&.fade-enter-active,
	&.fade-leave-active {
		transition: opacity .25s;
	}

	&.fade-enter,
	&.fade-leave-to {
		opacity: 0;
	}
}
</style>