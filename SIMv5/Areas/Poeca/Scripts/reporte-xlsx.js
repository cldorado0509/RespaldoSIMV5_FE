function crearReporteExcel(elementos) {
    var wb = XLSX.utils.book_new();

    /* make worksheet */
    var ws = XLSX.utils.json_to_sheet(elementos);

    ws.A1.v = "Sector";
    ws.B1.v = "Medida";
    ws.C1.v = "Acción";
    ws.D1.v = "Meta";
    ws.E1.v = "Cantidad Total";
    ws.F1.v = "Fecha";
    ws.G1.v = "Inversión";
    ws.H1.v = "Observaciones";

    /* Add the worksheet to the workbook */
    XLSX.utils.book_append_sheet(wb, ws, "Seguimientos Por Acción");

    var timestamp = new Date()

    XLSX.writeFile(wb, `reporte-seguimientos-${timestamp.getDate()}-${timestamp.getMonth()}-${timestamp.getFullYear()}.xlsx`);
}