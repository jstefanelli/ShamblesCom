<template>
    <button @click="$emit('click')" :class="{
            'p-3 text-center rounded-xl duration-300 hover:duration-300': true,
            ' bg-red-titlebar border-0 border-red-titlebar ': main,
            ' bg-bg border-2 border-bg-second hover:bg-bg-second': !main
        }">
        <slot></slot>
    </button>
</template>

<script lang="ts">

import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
    components: {

    }
})
export default class extends Vue {
    @Prop({default: false}) public main: boolean
}
</script>