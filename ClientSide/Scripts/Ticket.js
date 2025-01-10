document.addEventListener('DOMContentLoaded', function () {
    const { createApp, ref, computed } = Vue;

    const config = {
        setup() {
            const movies = ref([{ id: 1, name: "獅子王" }]);
            const theaters = ref([
                { id: 1, name: "影廳1" },
                { id: 2, name: "影廳2" },
                { id: 3, name: "影廳3" },
            ]);
            const dates = ref([]);
            const selectedMovie = ref(null);
            const selectedTheater = ref(null);
            const selectedDate = ref(null);
            const times = ref(["10:00", "13:00", "16:00", "19:00"]);
            const selectedTime = ref(null);
            const tickets = ref([
                { id: 1, name: "早鳥票" },
                { id: 2, name: "標準票" },
                { id: 3, name: "雙人票" },
                { id: 4, name: "單人票" },
                { id: 5, name: "午票" },
                { id: 6, name: "大頁票" },
            ]);
            const ticketTypes = ref([
                { id: 1, name: "全票", price: 300, quantity: 0 },
                { id: 2, name: "學生票", price: 250, quantity: 0 },
                { id: 3, name: "軍警票", price: 200, quantity: 0 },
            ]);
            const selectedTicket = ref(null);
            const selectedTicketType = ref(null);

            function generateDates() {
                const today = new Date();
                for (let i = 0; i < 6; i++) {
                    const date = new Date(today);
                    date.setDate(today.getDate() + i);
                    dates.value.push(date.toISOString().split("T")[0]);
                }
            }

            function selectMovie(movie) {
                selectedMovie.value = movie;
            }

            function selectTheater(theater) {
                selectedTheater.value = theater;
            }

            function selectDate(date) {
                selectedDate.value = date;
            }

            function selectTime(time) {
                selectedTime.value = time;
            }

            function selectSeats() {
                alert("選擇座位");
            }

            const totalPrice = computed(() => {
                let total = 0;
                if (selectedTicketType.value) {
                    total += selectedTicketType.value.price * selectedTicketType.value.quantity;
                }
                return total;
            });

            return {
                movies,
                theaters,
                dates,
                selectedMovie,
                selectedTheater,
                selectedDate,
                times,
                selectedTime,
                tickets,
                ticketTypes,
                selectedTicket,
                selectedTicketType,
                generateDates,
                selectMovie,
                selectTheater,
                selectDate,
                selectTime,
                selectSeats,
                totalPrice,
            };
        },
        created() {
            this.generateDates();
        },
        watch: {
            selectedTicket(newVal, oldVal) {
                this.selectedTicketType = null;
            },
            selectedTicketType(newVal, oldVal) {
                if (newVal) {
                    newVal.quantity = 0;
                }
            },
        },
    };

    createApp(config).mount("#app");
});
