<template>
	<base-layout v-bind="index">
		<div class="fill card no-hover padding-25 flex flex-vertical">
			<div class="flex align-center margin-b-25">
				<button class="passive" @click="goBack">
					Back
				</button>
				<span class="font-m margin-l-5">
					{{ profile.driver.nickname }}'s Profile:
				</span>
			</div>
			<div class="flex flex-grow fill-h">
				<div class="flex flex-vertical align-center flex-grow no-hover margin-r-25">
					<div class="font-m margin-b-5 fill-w">
						Profile image:
					</div>
					<div class="flex-grow relative fill-w" id="img_holder">

					</div>
					<div class="flex align-center fill-w">
						<input class="flex-grow margin-r-25" type="file" id="file_picker" name="file" />
						<button @click="uploadImage">Upload</button>
					</div>
				</div>
				<div class="flex flex-vertical flex-grow no-hover margin-r-25">
					<div class="font-m fill-w margin-b-5">
						Short description:
					</div>
					<textarea class="flex-grow-2 fill-w noresize margin-b-5" v-model="shortDescription">

					</textarea> 
					<div class="font-m fill-w margin-b-5">
						Full description:
					</div>
					<textarea class="flex-grow-3 fill-w noresize margin-b-5" v-model="description">

					</textarea>
					<div class="flex">
						<div class="flex-grow" />
						<button @click="submit">
							Save
						</button>
					</div>
				</div>
			</div>
		</div>
	</base-layout>
</template>

<script lang="ts">
import DriverProfile from '@/data/DriverProfile';
import SPA from '@/SPA/spa';
import { Vue, Prop, Watch, Component } from 'vue-property-decorator';
import BaseLayout from './baseLayout.vue';

@Component({
	components: {
		BaseLayout
		
	}
})
export default class extends Vue {
	@Prop({default: (): any => {}}) private index: any;
	@Prop({default: (): any => []}) private profile: DriverProfile;

	private description: string = "";
	private shortDescription: string = "";
	private resizeHandler: (ev: any) => boolean;

	private mounted() {
		this.description = this.profile.description;
		this.shortDescription = this.profile.shortDescription;

		let holder = document.getElementById("img_holder") as HTMLDivElement;
		let image = document.createElement("img");
		image.src = this.profile.imageLink;
		image.style.position = "absolute";
		image.style.height = holder.offsetHeight + "px";
		image.style.widows = (holder.offsetHeight * (10 / 13)) + "px";
		holder.appendChild(image);

		this.resizeHandler = (ev) => {
			image.style.height = holder.offsetHeight + "px";
			image.style.widows = (holder.offsetHeight * (10 / 13)) + "px";
			return true;
		};

		window.addEventListener("resize", this.resizeHandler);
	}

	private unmounted() {
		window.removeEventListener("resize", this.resizeHandler);
	}

	private goBack() {
		SPA.navigateAndRender("/admin/" + this.profile.seasonId + "/drivers");
	}

	private uploadImage() {
		let thiz = this;
		var fileInput = document.getElementById('file_picker') as HTMLInputElement;
		if (fileInput.files == null || fileInput.files.length == 0)
			return;

		var reader = new FileReader();
		reader.readAsDataURL(fileInput.files[0]);

		reader.onload = function () {
			let data = (reader.result as string);

			SPA.navigateAndRender("/admin/" + thiz.profile.seasonId + "/drivers/" + thiz.profile.driverId + "/pic", "POST", {
				base64data: data
			}, true);
		};
		reader.onerror = function (error) {
			console.log('Error: ', error);
		};
	}

	private submit() {
		let d : DriverProfile = {
			id: this.profile.id,
			driverId: this.profile.driverId,
			seasonId: this.profile.seasonId,
			shortDescription: this.shortDescription,
			description: this.description,
			imageLink: this.profile.imageLink
		};

		SPA.navigateAndRender("/admin/" + this.profile.seasonId + "/drivers/" + this.profile.driverId, "POST", d, true);
	}
}

</script>