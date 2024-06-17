document.addEventListener('DOMContentLoaded', function () {
    const filters = document.querySelectorAll('.filter');
    filters.forEach(filter => filter.addEventListener('change', filterPhotographers));

    function filterPhotographers() {
        const selectedFilters = {
            specialty: getSelectedFilters('specialty'),
            location: getSelectedFilters('location'),
            price: getSelectedFilters('price'),
            availability: getSelectedFilters('availability')
        };

        const photographers = document.querySelectorAll('.photographer-card');
        photographers.forEach(photographer => {
            const specialty = photographer.dataset.specialty;
            const location = photographer.dataset.location;
            const price = photographer.dataset.price;
            const availability = photographer.dataset.availability;

            if (
                (selectedFilters.specialty.length === 0 || selectedFilters.specialty.includes(specialty)) &&
                (selectedFilters.location.length === 0 || selectedFilters.location.includes(location)) &&
                (selectedFilters.price.length === 0 || selectedFilters.price.includes(price)) &&
                (selectedFilters.availability.length === 0 || selectedFilters.availability.includes(availability))
            ) {
                photographer.style.display = 'flex';
            } else {
                photographer.style.display = 'none';
            }
        });
    }

    function getSelectedFilters(filterName) {
        const selected = [];
        document.querySelectorAll(`input[name="${filterName}"]:checked`).forEach(checkbox => {
            selected.push(checkbox.value);
        });
        return selected;
    }
});
