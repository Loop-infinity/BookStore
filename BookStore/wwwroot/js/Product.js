var dataTable;

$(document).ready(function () {
    loadProductTable();
});


function loadProductTable() {
    dataTable = $('#product-table').DataTable({

        "ajax": {
            "url": "/Admin/Product/getAllProducts",
            "dataSrc": ""
        },

        "columns": [
            { "data": "name" },
            { "data": "isbn" },
            { "data": "author" },
            { "data": "price" },
            { "data": "listPrice" },
            { "data": "category.name"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick="Delete('/Admin/Product/Delete/${data}')" class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                }
            }


        ]
    });
}


function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
