(function ($) {
    /**
     *  Todo Bind UI Hadnler when the page ready.
     */
    $(document).ready(function () {
        // list table control
        $('#btn-clearSearch').bind('click', function () {
            clearSearch()
        })
        $('#select_goToPage').bind('change', function () {
            goToPage()
        })
        $('#select_changePageSize').bind('change', function () {
            changePageSize()
        })
    });

    // list table
    function clearSearch() {
        $('#input_number').val('')
        $('#input_name').val('')
        $('#select_goToPage').val(1);
        $('#form_search').submit()
    }
    function goToPage() {
        $('#form_search').submit()
    }
    function changePageSize() {
        $('#form_search').submit()
    }
    // ./list table
})(jQuery)