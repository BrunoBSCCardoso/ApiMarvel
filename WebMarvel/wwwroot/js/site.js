
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



//function GetHeroesToAutoComplete() {

//    var name = $("#input-searchHeroes").val();
//    name = (name == undefined ? "" : name);

//    if (name.length >= 3) {

//        $.ajax({
//            type: "POST",
//            url: "/Home/GetHeroesNameToAutoComplete",
//            dataType: "json",
//            data: { name: name },
//            success: function (response) {
//                alert("Sucess");
//            },
//            error: function (response) {

//            }
//        });
//    }
//}


