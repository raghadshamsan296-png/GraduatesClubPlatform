// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    $('.data-table').each(function () {
        if ($.fn.DataTable && !$.fn.DataTable.isDataTable(this)) {
            const tableElement = this;
            const table = $(this).DataTable({
                pageLength: 10,
                order: [],
                dom: $(this).data('hide-default-search') ? 'lrtip' : 'lfrtip',
                language: {
                    search: 'بحث:', lengthMenu: 'عرض _MENU_ سجل',
                    info: 'عرض _START_ إلى _END_ من _TOTAL_',
                    infoEmpty: 'لا توجد سجلات', emptyTable: 'لا توجد بيانات متاحة', zeroRecords: 'لا توجد نتائج مطابقة',
                    paginate: { first: 'الأول', last: 'الأخير', next: 'التالي', previous: 'السابق' }
                }
            });

            const filters = $('.table-filters[data-table-target="' + tableElement.id + '"]');

            filters.find('.js-table-search').on('input', function () {
                table.search(this.value).draw();
            });

            filters.find('.js-column-filter').on('change', function () {
                const value = this.value ? '^' + $.fn.dataTable.util.escapeRegex(this.value) + '$' : '';
                table.column(Number($(this).data('column'))).search(value, true, false).draw();
            });

            filters.find('.js-year-filter').on('change', function () {
                const value = this.value ? '^' + this.value + '/' : '';
                table.column(Number($(this).data('column'))).search(value, true, false).draw();
            });

            filters.find('.js-clear-filters').on('click', function () {
                filters.find('input').val('');
                filters.find('select').val('');
                table.search('');
                table.columns().search('');
                table.draw();
            });
        }
    });
});
