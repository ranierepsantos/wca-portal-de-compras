<template>
    <v-card>
        <v-card-title class="text-left text-primary text-h5">HISTÓRICO</v-card-title>
        <v-card-text>
          <v-row>
              <v-col class="text-left">
                  <v-timeline align="start" side="end">
                      <v-timeline-item dot-color="grey" size="small" v-for="item in eventos">
                          <div class="d-flex">
                              <div>
                                  <span class="text-caption" v-html="moment(item.dataEvento).format('DD/MM/YYYY HH:mm:ss') + ' - ' + item.evento"></span>
                              </div>
                          </div>
                      </v-timeline-item>
                  </v-timeline>
              </v-col>
          </v-row>
        </v-card-text>
      </v-card>
</template>

<script setup>
    import moment from 'moment';
    defineProps({
        eventos: {
            type: Array,
            default: function() {return []}
        }
    })
</script>