<template>
	<base-layout v-bind="index">
		<card class="w-fll min-h-screen p-6 flex flex-col">
			<div class="flex items-center mb-6">
				<Mybutton class="passive" @click="goBack">
					Back
				</Mybutton>
				<span class="text-xl ml-1">
					{{ profile.driver.nickname }}'s Profile:
				</span>
			</div>
			<div class="flex flex-grow h-full">
				<div class="flex flex-col items-center flex-grow mr-6">
					<div class="text-xl mb-1 w-full">
						Profile image:
					</div>
					<div class="flex-grow relative w-full" id="img_holder">
						<img class="absolute inset-0 object-contain w-full h-full object-right-bottom overflow-hidden" :src="profile.imageLink">
					</div>
					<div class="flex items-center w-full">
						<Myinput class="flex-grow margin-r-25" type="file" id="file_picker" name="file"></Myinput>
						<Mybutton @click="uploadImage">Upload</Mybutton>
					</div>
				</div>
				<div class="flex flex-col flex-grow mr-6">
					<div class="font-m w-full mb-2">
						Short description:
					</div>
					<Mytextarea class="flex-grow-[2] w-full resize-none mb-2" v-model="shortDescription">

					</Mytextarea> 
					<div class="font-m w-full mb-2">
						Full description:
					</div>
					<Mytextarea class="flex-grow-[3] w-full resize-none mb-2" v-model="description">

					</Mytextarea>
					<div class="flex">
						<div class="flex-grow" ></div>
						<Mybutton :main="true" @click="submit">
							Save
						</Mybutton>
					</div>
				</div>
			</div>
		</card>
	</base-layout>
</template>

<script lang="ts">
import DriverProfile from '@/data/DriverProfile';
import SPA from '@/SPA/spa';
import { Vue, Prop, Watch, Component } from 'vue-property-decorator';
import BaseLayout from './baseLayout.vue';
import Myinput from '../layout/controls/myinput.vue';
import Mybutton from '../layout/controls/mybutton.vue';
import Section from '../layout/section.vue';
import Mytextarea from '../layout/controls/mytextarea.vue';

@Component({
	components: {
		BaseLayout,
		Myinput,
		Mybutton,
		"card": Section,
		Mytextarea
	}
})
export default class extends Vue {
	@Prop({default: (): any => {}}) public index: any;
	@Prop({default: (): any => []}) public profile: DriverProfile;

	public description: string = "";
	public shortDescription: string = "";
	public resizeHandler: (ev: any) => boolean;

	private mounted() {
		this.description = this.profile.description;
		this.shortDescription = this.profile.shortDescription;
	}

	public unmounted() {
		if (this.resizeHandler) {
			window.removeEventListener("resize", this.resizeHandler);
		}
	}

	public goBack() {
		SPA.navigateAndRender("/admin/" + this.profile.seasonId + "/drivers");
	}

	public uploadImage() {
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

	public submit() {
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