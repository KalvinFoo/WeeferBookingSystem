$(".number").focusin(function () {
	if ($(this).val() == '0' || $(this).val() == '0.00') {
		$(this).val('');
	}
});
$(".number").focusout(function () {
	if ($(this).val() == '') {
		$(this).val('0');
	}
});

var MyApp = function () {
	return {
		dataTables: function () {
			var handleInit = function (options) {
				options = $.extend(true, {
					"unorderableColumn": [0, -1],
					"defaultOrder": [[1, "asc"]],
					"ordering": true,
				}, options);

				var grid = new Datatable();

				grid.init({
					src: $("#" + options.id),
					onSuccess: function (grid) {

					},
					onError: function (grid) {

					},
					loadingMessage: 'Loading...',
					dataTable: {
						"ajax": {
							"url": options.src,
						},
						"lengthMenu": [
							[10, 20, 50, 100, 150, -1],
							[10, 20, 50, 100, 150, "All"]
						],
						"pageLength": 10,
						"columnDefs": [{
							'orderable': false,
							'targets': options.unorderableColumn
						}],
						"order": options.defaultOrder,
						"ordering": options.ordering,
						"dom": "<'row'<'col-md-7 col-sm-12'pli><'col-md-5 col-sm-12'<'table-group-actions pull-right'>>r><'table-scrollable't><'row'<'col-md-7 col-sm-12'pli><'col-md-5 col-sm-12'>>",
					}
				});
				a = grid;

				grid.getTableWrapper().on('click', '.table-group-action-submit', function (e) {
					e.preventDefault();
					var action = $(".table-group-action-input", grid.getTableWrapper());
					if (action.val() != "" && grid.getSelectedRowsCount() > 0) {
						grid.setAjaxParam("customActionType", "group_action");
						grid.setAjaxParam("customActionName", action.val());
						grid.setAjaxParam("id", grid.getSelectedRows());
						grid.getDataTable().ajax.reload();
						grid.clearAjaxParams();
					} else if (action.val() == "") {
						Metronic.alert({
							type: 'danger',
							icon: 'warning',
							message: 'Please select an action',
							container: grid.getTableWrapper(),
							place: 'prepend'
						});
					} else if (grid.getSelectedRowsCount() === 0) {
						Metronic.alert({
							type: 'danger',
							icon: 'warning',
							message: 'No record selected',
							container: grid.getTableWrapper(),
							place: 'prepend'
						});
					}
				});
			};

			return {
				init: function (options) {
					handleInit(options);
				}
			}
		},
		datePicker: function () {
			$('.date-picker').datepicker({
				rtl: App.isRTL(),
				format: "d-M-yyyy",
				orientation: "left",
				todayBtn: true,
				autoclose: true,
				pickerPosition: (App.isRTL() ? "bottom-right" : "bottom-left")
			});
		},
		dateTimePicker: function () {
			$('.datetime-picker').datetimepicker({
				rtl: App.isRTL(),
				format: "d-M-yyyy hh:ii ",
				orientation: "left",
				todayBtn: true,
				autoclose: true,
				pickerPosition: (App.isRTL() ? "bottom-right" : "bottom-left")
			});
		}
	}
}