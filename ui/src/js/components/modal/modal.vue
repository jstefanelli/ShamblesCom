<template>
	<transition name="fade">
		<div class="modal" v-if="visible">
			<div class="flex align-start">
				<div class="flex-grow" />
				<div class="modal-main flex flex-vertical margin-t-25">
					<div class="modal-header font-m margin-b-25">
						<slot name="header" />
					</div>

					<div class="modal-content flex-grow margin-b-25">
						<slot />
					</div>

					<div class="modal-footer flex align-center">
						<slot name="footer-heading" />
						<div class="flex-grow" />
						<slot name="footer" />
					</div>
				</div>
				<div class="flex-grow" />
			</div>
		</div>
	</transition>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator';

@Component({
	components: {

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
@import "#/colors";

.modal {
	position: fixed;
	overflow-x: hidden;
	overflow-y: auto;
	top: 0;
	left: 0;
	right: 0;
	bottom: 0;
	background-color: transparentize($bg-color-dark, .5);

	&.fade-enter-active,
	&.fade-leave-active {
		transition: opacity .25s;
	}

	&.fade-enter,
	&.fade-leave-to {
		opacity: 0;
	}

	.modal-main {
		border-top-right-radius: 10px;
		border-bottom-left-radius: 10px;
		min-width: 600px;
		padding: 25px;
		background-color: $bg-color;
		border-right: 5px solid white;
		border-top: 5px solid white;
	}

	.modal-footer {
		* {
			margin-left: 10px;
		}
	}
}
</style>