const API_BASE_URL = 'https://localhost:64330';

let equipos = [];
let categorias = [];
let prestamos = [];
let revisiones = [];

function showLoading(tablaId) {
    document.getElementById(tablaId).innerHTML =
        `<tr><td colspan="10" class="text-center py-3">
            <div class="spinner-border spinner-border-sm text-success me-2"></div>
            Cargando...
         </td></tr>`;
}

function showError(mensaje) {
    Swal.fire({ icon: 'error', title: 'Error', text: mensaje, confirmButtonColor: '#2C5F2D' });
}

function showSuccess(mensaje) {
    Swal.fire({ icon: 'success', title: '¡Listo!', text: mensaje, timer: 1500, showConfirmButton: false });
}

function estadoBadge(estado) {
    const colores = {
        'disponible':       'success',
        'prestado':         'warning',
        'en mantenimiento': 'info',
        'dañado':           'danger',
        'activo':           'primary',
        'devuelto':         'secondary',
        'vencido':          'danger',
        'aprobado':         'success',
        'en reparacion':    'warning',
        'dado de baja':     'danger',
        'ninguno':          'secondary',
        'leve':             'info',
        'moderado':         'warning',
        'grave':            'danger',
    };
    const color = colores[estado] || 'secondary';
    return `<span class="badge bg-${color}">${estado}</span>`;
}

async function fetchData(endpoint) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`);
        if (!response.ok) throw new Error(`HTTP ${response.status}`);
        return await response.json();
    } catch (error) {
        showError('No se pudo conectar con el servidor. ¿Está corriendo CampTrack.API?');
        return [];
    }
}

async function postData(endpoint, data) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        if (!response.ok) {
            const err = await response.json().catch(() => ({}));
            throw new Error(err.message || `HTTP ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        showError(error.message || 'Error al guardar los datos.');
        return null;
    }
}

async function putData(endpoint, data) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        if (!response.ok) throw new Error(`HTTP ${response.status}`);
        return await response.json();
    } catch (error) {
        showError('Error al actualizar los datos.');
        return null;
    }
}

async function deleteData(endpoint) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, { method: 'DELETE' });
        return response.ok;
    } catch (error) {
        showError('Error al eliminar.');
        return false;
    }
}

async function cargarEquipos() {
    showLoading('tablaEquipos');
    equipos = await fetchData('/api/equipos');
    renderEquipos(equipos);
}

function renderEquipos(lista) {
    const tbody = document.getElementById('tablaEquipos');
    if (!lista.length) {
        tbody.innerHTML = '<tr><td colspan="6" class="text-center text-muted">No hay equipos registrados.</td></tr>';
        return;
    }
    tbody.innerHTML = lista.map(e => `
        <tr>
            <td><strong>${e.nombre}</strong></td>
            <td class="text-muted small">${e.descripcion || '—'}</td>
            <td><code>${e.codigoSerial || '—'}</code></td>
            <td>$${(e.costo || 0).toFixed(2)}</td>
            <td>${estadoBadge(e.estado)}</td>
            <td>
                <button class="btn btn-outline-secondary btn-sm me-1" onclick="editarEquipo(${e.id})">
                    <i class="bi bi-pencil"></i>
                </button>
                <button class="btn btn-outline-danger btn-sm" onclick="eliminarEquipo(${e.id})">
                    <i class="bi bi-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');
}

function buscarEquipos() {
    const q = document.getElementById('buscarEquipo').value.toLowerCase();
    const filtrados = equipos.filter(e => e.nombre.toLowerCase().includes(q));
    renderEquipos(filtrados);
}

function filtrarEquipos() {
    const estado = document.getElementById('filtroEstado').value;
    const filtrados = estado ? equipos.filter(e => e.estado === estado) : equipos;
    renderEquipos(filtrados);
}

function abrirModalEquipo() {
    document.getElementById('equipoId').value = '';
    document.getElementById('equipoNombre').value = '';
    document.getElementById('equipoDescripcion').value = '';
    document.getElementById('equipoSerial').value = '';
    document.getElementById('equipoCosto').value = '';
    document.getElementById('equipoEstado').value = 'disponible';
    document.getElementById('tituloModalEquipo').textContent = 'Nuevo Equipo';

    const sel = document.getElementById('equipoCategoriaId');
    sel.innerHTML = '<option value="">Selecciona categoría...</option>';
    categorias.forEach(c => {
        sel.innerHTML += `<option value="${c.id}">${c.nombre}</option>`;
    });

    new bootstrap.Modal(document.getElementById('modalEquipo')).show();
}

async function editarEquipo(id) {
    const equipo = equipos.find(e => e.id === id);
    if (!equipo) return;

    document.getElementById('equipoId').value = equipo.id;
    document.getElementById('equipoNombre').value = equipo.nombre;
    document.getElementById('equipoDescripcion').value = equipo.descripcion || '';
    document.getElementById('equipoSerial').value = equipo.codigoSerial || '';
    document.getElementById('equipoCosto').value = equipo.costo || 0;
    document.getElementById('equipoEstado').value = equipo.estado || 'disponible';
    document.getElementById('tituloModalEquipo').textContent = 'Editar Equipo';

    const sel = document.getElementById('equipoCategoriaId');
    sel.innerHTML = '<option value="">Selecciona categoría...</option>';
    categorias.forEach(c => {
        sel.innerHTML += `<option value="${c.id}" ${c.id === equipo.categoriaId ? 'selected' : ''}>${c.nombre}</option>`;
    });

    new bootstrap.Modal(document.getElementById('modalEquipo')).show();
}

async function guardarEquipo() {
    const id = document.getElementById('equipoId').value;
    const nombre = document.getElementById('equipoNombre').value.trim();
    if (!nombre) { showError('El nombre es obligatorio.'); return; }

    const estadoSeleccionado = document.getElementById('equipoEstado').value;
    const data = {
        nombre,
        descripcion:  document.getElementById('equipoDescripcion').value,
        codigoSerial: document.getElementById('equipoSerial').value,
        categoriaId:  parseInt(document.getElementById('equipoCategoriaId').value) || 1,
        costo:        parseFloat(document.getElementById('equipoCosto').value) || 0,
        estado:       estadoSeleccionado,
        disponible:   estadoSeleccionado === 'disponible',
    };

    const resultado = id
        ? await putData(`/api/equipos/${id}`, data)
        : await postData('/api/equipos', data);

    if (resultado) {
        bootstrap.Modal.getInstance(document.getElementById('modalEquipo')).hide();
        showSuccess(id ? 'Equipo actualizado.' : 'Equipo creado.');
        cargarEquipos();
    }
}

async function eliminarEquipo(id) {
    const confirm = await Swal.fire({
        title: '¿Eliminar equipo?',
        text: 'Esta acción no se puede deshacer.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#2C5F2D',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    });
    if (confirm.isConfirmed) {
        const ok = await deleteData(`/api/equipos/${id}`);
        if (ok) { showSuccess('Equipo eliminado.'); cargarEquipos(); }
    }
}

async function cargarCategorias() {
    showLoading('tablaCategorias');
    categorias = await fetchData('/api/categorias');
    renderCategorias(categorias);
}

function buscarCategorias() {
    const q = document.getElementById('buscarCategoria').value.toLowerCase();
    const filtradas = categorias.filter(c =>
        c.nombre.toLowerCase().includes(q) ||
        (c.descripcion || '').toLowerCase().includes(q)
    );
    renderCategorias(filtradas);
}

function renderCategorias(lista) {
    const tbody = document.getElementById('tablaCategorias');
    if (!lista.length) {
        tbody.innerHTML = '<tr><td colspan="4" class="text-center text-muted">No hay categorías.</td></tr>';
        return;
    }
    tbody.innerHTML = lista.map(c => `
        <tr>
            <td><strong>${c.nombre}</strong></td>
            <td class="text-muted small">${c.descripcion || '—'}</td>
            <td>${c.isActive ? '<span class="badge bg-success">Activa</span>' : '<span class="badge bg-secondary">Inactiva</span>'}</td>
            <td>
                <button class="btn btn-outline-secondary btn-sm me-1" onclick="editarCategoria(${c.id})">
                    <i class="bi bi-pencil"></i>
                </button>
                <button class="btn btn-outline-danger btn-sm" onclick="eliminarCategoria(${c.id})">
                    <i class="bi bi-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');
}

function abrirModalCategoria() {
    document.getElementById('categoriaId').value = '';
    document.getElementById('categoriaNombre').value = '';
    document.getElementById('categoriaDescripcion').value = '';
    document.getElementById('tituloModalCategoria').textContent = 'Nueva Categoría';
    new bootstrap.Modal(document.getElementById('modalCategoria')).show();
}

async function editarCategoria(id) {
    const cat = categorias.find(c => c.id === id);
    if (!cat) return;
    document.getElementById('categoriaId').value = cat.id;
    document.getElementById('categoriaNombre').value = cat.nombre;
    document.getElementById('categoriaDescripcion').value = cat.descripcion || '';
    document.getElementById('tituloModalCategoria').textContent = 'Editar Categoría';
    new bootstrap.Modal(document.getElementById('modalCategoria')).show();
}

async function guardarCategoria() {
    const id = document.getElementById('categoriaId').value;
    const nombre = document.getElementById('categoriaNombre').value.trim();
    if (!nombre) { showError('El nombre es obligatorio.'); return; }

    const data = { nombre, descripcion: document.getElementById('categoriaDescripcion').value };
    const resultado = id
        ? await putData(`/api/categorias/${id}`, data)
        : await postData('/api/categorias', data);

    if (resultado) {
        bootstrap.Modal.getInstance(document.getElementById('modalCategoria')).hide();
        showSuccess(id ? 'Categoría actualizada.' : 'Categoría creada.');
        cargarCategorias();
    }
}

async function eliminarCategoria(id) {
    const confirm = await Swal.fire({
        title: '¿Eliminar categoría?', icon: 'warning',
        showCancelButton: true, confirmButtonColor: '#d33',
        confirmButtonText: 'Sí', cancelButtonText: 'No'
    });
    if (confirm.isConfirmed) {
        const ok = await deleteData(`/api/categorias/${id}`);
        if (ok) { showSuccess('Categoría eliminada.'); cargarCategorias(); }
    }
}

async function cargarPrestamos() {
    showLoading('tablaPrestamos');
    prestamos = await fetchData('/api/prestamos');
    renderPrestamos(prestamos);
}

function buscarPrestamos() {
    const q = document.getElementById('buscarPrestamo').value.toLowerCase();
    const estado = document.getElementById('filtroPrestamo').value;
    const filtrados = prestamos.filter(p => {
        const coincideTexto = !q ||
            (p.usuarioNombre || '').toLowerCase().includes(q) ||
            (p.equipoNombre || '').toLowerCase().includes(q);
        const coincideEstado = !estado || p.estado === estado;
        return coincideTexto && coincideEstado;
    });
    renderPrestamos(filtrados);
}

function renderPrestamos(lista) {
    const tbody = document.getElementById('tablaPrestamos');
    if (!lista.length) {
        tbody.innerHTML = '<tr><td colspan="7" class="text-center text-muted">No hay préstamos.</td></tr>';
        return;
    }
    tbody.innerHTML = lista.map(p => `
        <tr>
            <td><strong>${p.usuarioNombre}</strong></td>
            <td class="small">${p.usuarioContacto || '—'}</td>
            <td><strong>${p.equipoNombre || '—'}</strong></td>
            <td class="small">${p.fechaSalida ? p.fechaSalida.substring(0,10) : '—'}</td>
            <td class="small">${p.fechaRetornoEsperada ? p.fechaRetornoEsperada.substring(0,10) : '—'}</td>
            <td>${estadoBadge(p.estado)}</td>
            <td>
                ${p.estado === 'activo' ? `
                    <button class="btn btn-outline-success btn-sm me-1" onclick="devolverPrestamo(${p.id})">
                        <i class="bi bi-arrow-return-left"></i> Devolver
                    </button>` : ''}
                <button class="btn btn-outline-danger btn-sm" onclick="eliminarPrestamo(${p.id})">
                    <i class="bi bi-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');
}

function abrirModalPrestamo() {
    ['prestamoUsuario','prestamoContacto','prestamoFechaRetorno','prestamoObservaciones']
        .forEach(id => document.getElementById(id).value = '');

    const sel = document.getElementById('prestamoEquipoId');
    sel.innerHTML = '<option value="">Selecciona un equipo disponible...</option>';
    equipos.filter(e => e.disponible).forEach(e => {
        sel.innerHTML += `<option value="${e.id}">${e.nombre} (${e.codigoSerial || 'S/N'})</option>`;
    });

    new bootstrap.Modal(document.getElementById('modalPrestamo')).show();
}

async function guardarPrestamo() {
    const equipoId = document.getElementById('prestamoEquipoId').value;
    const usuario  = document.getElementById('prestamoUsuario').value.trim();
    if (!equipoId || !usuario) { showError('Equipo y usuario son obligatorios.'); return; }

    const data = {
        equipoId:             parseInt(equipoId),
        usuarioNombre:        usuario,
        usuarioContacto:      document.getElementById('prestamoContacto').value,
        fechaRetornoEsperada: document.getElementById('prestamoFechaRetorno').value || null,
        observaciones:        document.getElementById('prestamoObservaciones').value
    };
    const resultado = await postData('/api/prestamos', data);
    if (resultado) {
        bootstrap.Modal.getInstance(document.getElementById('modalPrestamo')).hide();
        showSuccess('Préstamo registrado.');
        cargarPrestamos();
        cargarEquipos();
    }
}

async function devolverPrestamo(id) {
    const confirm = await Swal.fire({
        title: '¿Marcar como devuelto?', icon: 'question',
        showCancelButton: true, confirmButtonColor: '#2C5F2D',
        confirmButtonText: 'Sí, devuelto', cancelButtonText: 'Cancelar'
    });
    if (confirm.isConfirmed) {
        const ok = await putData(`/api/prestamos/${id}/devolver`, {});
        if (ok) { showSuccess('Préstamo marcado como devuelto.'); cargarPrestamos(); cargarEquipos(); }
    }
}

async function eliminarPrestamo(id) {
    const confirm = await Swal.fire({
        title: '¿Eliminar préstamo?', icon: 'warning',
        showCancelButton: true, confirmButtonColor: '#d33',
        confirmButtonText: 'Sí', cancelButtonText: 'No'
    });
    if (confirm.isConfirmed) {
        const ok = await deleteData(`/api/prestamos/${id}`);
        if (ok) { showSuccess('Préstamo eliminado.'); cargarPrestamos(); }
    }
}

async function cargarRevisiones() {
    showLoading('tablaRevisiones');
    revisiones = await fetchData('/api/revisiones');
    renderRevisiones(revisiones);
}

function buscarRevisiones() {
    const q = document.getElementById('buscarRevision').value.toLowerCase();
    const resultado = document.getElementById('filtroRevision').value;
    const filtradas = revisiones.filter(r => {
        const coincideTexto = !q ||
            (r.equipoNombre || '').toLowerCase().includes(q) ||
            (r.tecnico || '').toLowerCase().includes(q);
        const coincideResultado = !resultado || r.resultado === resultado;
        return coincideTexto && coincideResultado;
    });
    renderRevisiones(filtradas);
}

function renderRevisiones(lista) {
    const tbody = document.getElementById('tablaRevisiones');
    if (!lista.length) {
        tbody.innerHTML = '<tr><td colspan="7" class="text-center text-muted">No hay revisiones.</td></tr>';
        return;
    }
    tbody.innerHTML = lista.map(r => `
        <tr>
            <td><strong>${r.equipoNombre || '—'}</strong></td>
            <td>${r.tecnico}</td>
            <td class="small">${r.fecha ? r.fecha.substring(0,10) : '—'}</td>
            <td>${estadoBadge(r.nivelDano)}</td>
            <td>$${(r.costoReparacion || 0).toFixed(2)}</td>
            <td>${estadoBadge(r.resultado)}</td>
            <td>
                <button class="btn btn-outline-danger btn-sm" onclick="eliminarRevision(${r.id})">
                    <i class="bi bi-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');
}

function abrirModalRevision() {
    ['revisionTecnico','revisionDanos'].forEach(id => document.getElementById(id).value = '');
    document.getElementById('revisionCosto').value = '0';
    document.getElementById('revisionNivelDano').value = 'ninguno';
    document.getElementById('revisionResultado').value = 'aprobado';

    const sel = document.getElementById('revisionEquipoId');
    sel.innerHTML = '<option value="">Selecciona un equipo...</option>';
    equipos.forEach(e => {
        sel.innerHTML += `<option value="${e.id}">${e.nombre} — ${e.estado}</option>`;
    });

    new bootstrap.Modal(document.getElementById('modalRevision')).show();
}

async function guardarRevision() {
    const equipoId = document.getElementById('revisionEquipoId').value;
    const tecnico  = document.getElementById('revisionTecnico').value.trim();
    if (!equipoId || !tecnico) { showError('Equipo y técnico son obligatorios.'); return; }

    const data = {
        equipoId:         parseInt(equipoId),
        tecnico,
        descripcionDanos: document.getElementById('revisionDanos').value,
        nivelDano:        document.getElementById('revisionNivelDano').value,
        costoReparacion:  parseFloat(document.getElementById('revisionCosto').value) || 0,
        resultado:        document.getElementById('revisionResultado').value
    };
    const resultado = await postData('/api/revisiones', data);
    if (resultado) {
        bootstrap.Modal.getInstance(document.getElementById('modalRevision')).hide();
        showSuccess('Revisión registrada.');
        cargarRevisiones();
        cargarEquipos();
    }
}

async function eliminarRevision(id) {
    const confirm = await Swal.fire({
        title: '¿Eliminar revisión?', icon: 'warning',
        showCancelButton: true, confirmButtonColor: '#d33',
        confirmButtonText: 'Sí', cancelButtonText: 'No'
    });
    if (confirm.isConfirmed) {
        const ok = await deleteData(`/api/revisiones/${id}`);
        if (ok) { showSuccess('Revisión eliminada.'); cargarRevisiones(); }
    }
}

document.addEventListener('DOMContentLoaded', function () {
    cargarEquipos();
    cargarCategorias();
    document.querySelector('[href="#categorias"]').addEventListener('shown.bs.tab', cargarCategorias);
    document.querySelector('[href="#prestamos"]').addEventListener('shown.bs.tab', cargarPrestamos);
    document.querySelector('[href="#revisiones"]').addEventListener('shown.bs.tab', cargarRevisiones);
});
