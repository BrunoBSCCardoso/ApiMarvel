
$(document).keypress(function (e) {
    if (e.which == 13) {
        GetHeroesByName();
    }
});


function GetHeroesByName() {

    var name = $("#input-searchHeroes").val();

    $.ajax({
        type: "POST",
        url: "/Home/GetHeroesByName",
        dataType: "html",
        data: { name: name },
        success: function (response) {
            $("#div-list-heroes").html(response);
        },
        error: function (response) {

        }
    });
}

$('#input-searchHeroes').autocomplete({
    serviceUrl: '/Home/GetHeroesByName',
    onSelect: function (suggestion) {
        
    }
});


