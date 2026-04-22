// ===== Config API =====
const API_URL = "/Evaluation";

// ===== DOM Elements =====
const loadBtn = document.getElementById("load-btn");
const progressContainer = document.getElementById("progress-container");
const progressBar = document.getElementById("progress-bar");
const progressLabel = document.getElementById("progress-label");
const summaryEl = document.getElementById("summary");
const tableBody = document.querySelector("#test-table tbody");
const lastUpdatedEl = document.getElementById("last-updated");
const jsonOutput = document.getElementById("jsonOutput");
const downloadJsonBtn = document.getElementById("download-json-btn");
const filterStatusSelect = document.getElementById("filter-status");
const searchBox = document.getElementById("search-box");
const prevPageBtn = document.getElementById("prev-page-btn");
const nextPageBtn = document.getElementById("next-page-btn");
const pageInfo = document.getElementById("page-info");
const rowsPerPageSelect = document.getElementById("rows-per-page");

// ===== State =====
let data = null;
let filteredData = [];
let progressTimer = null;
let currentPage = 1;
let rowsPerPage = 10;

// ===== Progress Bar =====
function startProgress() {
    let p = 0;
    if (!progressBar) return;

    progressBar.style.width = "0%";
    progressTimer = setInterval(() => {
        if (p < 90) {
            p += 5;
            progressBar.style.width = p + "%";
        }
    }, 200);
}

function stopProgress() {
    if (progressTimer) {
        clearInterval(progressTimer);
        progressTimer = null;
    }
    if (progressBar) {
        progressBar.style.width = "100%";
    }
}

function setLoading(isLoading, text = "Loading...") {
    if (!progressContainer || !progressLabel || !loadBtn) return;

    if (isLoading) {
        loadBtn.disabled = true;
        progressContainer.style.display = "flex";
        progressLabel.textContent = text;
        startProgress();
    } else {
        loadBtn.disabled = false;
        progressContainer.style.display = "none";
        stopProgress();
    }
}

// ===== Call API =====
async function loadDataFromApi() {
    setLoading(true, "Loading data from API...");

    try {
        const res = await fetch(API_URL, {
            method: "GET",
            headers: {
                accept: "text/plain"
            }
        });

        if (!res.ok) {
            throw new Error(`HTTP ${res.status}`);
        }

        const text = await res.text();
        const trimmed = text.trim();
        const parsed = JSON.parse(trimmed);

        data = Array.isArray(parsed) ? parsed : [parsed];
        console.log("Data loaded:", data);

        if (jsonOutput) {
            jsonOutput.textContent = JSON.stringify(data, null, 2);
        }

        // Reset pagination and filters
        currentPage = 1;
        filterAndRenderData();
        updateLastUpdated();

        setLoading(false);
        return parsed;
    } catch (error) {
        console.error("Error loading data:", error);
        alert("Error loading data: " + error.message);
        setLoading(false);
    }
}

// ===== Filter and Render =====
function filterAndRenderData() {
    if (!data) return;

    // Apply filters
    filteredData = data.filter(item => {
        const status = filterStatusSelect.value;
        const searchTerm = searchBox.value.toLowerCase();

        // Filter by status
        if (status) {
            const itemStatus = item.runComplete ? "PASS" : "FAIL";
            if (itemStatus !== status) return false;
        }

        // Filter by search term
        if (searchTerm) {
            const testCaseId = (item.testCaseId || "").toLowerCase();
            if (!testCaseId.includes(searchTerm)) return false;
        }

        return true;
    });

    currentPage = 1; // Reset to first page
    createSummaryCards(data); // Show summary for ALL data
    renderTablePage();
}

// ===== Pagination =====
function renderTablePage() {
    if (!tableBody) return;

    const totalRows = filteredData.length;
    const totalPages = Math.ceil(totalRows / rowsPerPage);

    // Validate current page
    if (currentPage < 1) currentPage = 1;
    if (currentPage > totalPages && totalPages > 0) currentPage = totalPages;

    // Calculate start and end indexes
    const startIdx = (currentPage - 1) * rowsPerPage;
    const endIdx = Math.min(startIdx + rowsPerPage, totalRows);
    const pageData = filteredData.slice(startIdx, endIdx);

    // Clear table
    tableBody.innerHTML = "";

    // Render rows
    pageData.forEach((item, idx) => {
        const globalIdx = startIdx + idx;
        createTableRow(item, globalIdx);
    });

    // Update pagination info
    if (pageInfo) {
        pageInfo.textContent = `Page ${currentPage} / ${totalPages} (${totalRows} items)`;
    }

    // Update button states
    if (prevPageBtn) prevPageBtn.disabled = currentPage <= 1;
    if (nextPageBtn) nextPageBtn.disabled = currentPage >= totalPages;
}

function createTableRow(item, rowIndex) {
    const mainRow = document.createElement("tr");
    mainRow.className = "data-row";

    const isComplete = item.runComplete ?? false;
    const statusBadge = isComplete ?
        '<span class="badge badge-pass">PASS</span>' :
        '<span class="badge badge-fail">FAIL</span>';

    // Tools info
    const tools = item.tools || {};
    const toolsInfo = `
        <div class="info-cell">
            <span class="label">Invoked:</span> <span class="value">${tools.invokedCount || 0}</span><br>
            <span class="label">Wrong:</span> <span class="value">${tools.wrongInvocations || 0}</span><br>
            <span class="label">Compliance:</span> <span class="value">${tools.finalCompliance ? '✓' : '✗'}</span>
        </div>
    `;

    // Replanning info
    const replanning = item.replanning || {};
    const replanInfo = `
        <div class="info-cell">
            <span class="label">History:</span> <span class="value">${replanning.planHistoryCount || 0}</span><br>
            <span class="label">Replan Count:</span> <span class="value">${replanning.replanCount || 0}</span><br>
            <span class="label">Status:</span> <span class="value">${replanning.replanned ? 'Yes' : 'No'}</span>
        </div>
    `;

    mainRow.innerHTML = `
        <td><strong>${htmlEscape(item.testCaseId || `Row-${rowIndex + 1}`)}</strong></td>
        <td class="center">${statusBadge}</td>
        <td>${toolsInfo}</td>
        <td>${replanInfo}</td>
        <td>
            <button class="btn-toggle" data-index="${rowIndex}">
                Detail
            </button>
        </td>
    `;

    tableBody.appendChild(mainRow);

    // Add detail row
    const detailRow = document.createElement("tr");
    detailRow.className = "detail-row";
    detailRow.style.display = "none";
    detailRow.innerHTML = createDetailContent(item);
    tableBody.appendChild(detailRow);

    // Toggle detail on button click
    const toggleBtn = mainRow.querySelector(".btn-toggle");
    toggleBtn.addEventListener("click", () => {
        const isVisible = detailRow.style.display !== "none";
        detailRow.style.display = isVisible ? "none" : "table-row";
        toggleBtn.classList.toggle("expanded");
    });
}

function createDetailContent(item) {
    const tools = item.tools || {};
    const replanning = item.replanning || {};

    const usedTools = tools.usedSet && tools.usedSet.length > 0 ?
        `<div class="detail-subsection"><strong>Tools Used:</strong><ul class="tools-list">${tools.usedSet.map(t => `<li>${htmlEscape(t)}</li>`).join('')}</ul></div>` :
        '<div class="detail-subsection"><em>(No tools used)</em></div>';

    const violations = tools.violations && tools.violations.length > 0 ?
        `<div class="detail-subsection"><strong>Violations:</strong><pre>${JSON.stringify(tools.violations, null, 2)}</pre></div>` :
        '';

    return `
        <td colspan="5" class="detail-content">
            <div class="detail-panel">
                <div class="detail-section">
                    <h4>Tools Execution Details</h4>
                    <table class="detail-table">
                        <tr><td>WTR Step:</td><td>${tools.wtrStep || 0}</td></tr>
                        <tr><td>Required Coverage:</td><td>${tools.requiredCoverage || 0}</td></tr>
                        <tr><td>Forbidden Used:</td><td>${tools.forbiddenUsed ? 'Yes' : 'No'}</td></tr>
                        <tr><td>Final Compliance:</td><td>${tools.finalCompliance ? 'Compliant' : 'Non-Compliant'}</td></tr>
                        <tr><td>Replan Count:</td><td>${tools.replanCount || 0}</td></tr>
                        <tr><td>Invoked Count:</td><td>${tools.invokedCount || 0}</td></tr>
                        <tr><td>Wrong Invocations:</td><td>${tools.wrongInvocations || 0}</td></tr>
                    </table>
                    ${usedTools}
                    ${violations}
                </div>

                <div class="detail-section">
                    <h4>Replanning Details</h4>
                    <table class="detail-table">
                        <tr><td>Plan History Count:</td><td>${replanning.planHistoryCount || 0}</td></tr>
                        <tr><td>Replan Count:</td><td>${replanning.replanCount || 0}</td></tr>
                        <tr><td>Was Replanned:</td><td>${replanning.replanned ? 'Yes' : 'No'}</td></tr>
                    </table>
                </div>

                <div class="detail-section">
                    <h4>Raw JSON</h4>
                    <pre class="json-view">${JSON.stringify(item, null, 2)}</pre>
                </div>
            </div>
        </td>
    `;
}

// ===== Summary Cards =====
function createSummaryCards(list) {
    if (!summaryEl) return;
    if (!Array.isArray(list) || list.length === 0) return;

    const total = list.length;
    const passedCount = list.filter(d => d.runComplete).length;

    // Count tool violations
    let totalWrongInvocations = 0;
    let totalReplanCount = 0;
    let totalInvokedCount = 0;

    list.forEach(item => {
        if (item.tools) {
            totalWrongInvocations += item.tools.wrongInvocations || 0;
            totalReplanCount += item.tools.replanCount || 0;
            totalInvokedCount += item.tools.invokedCount || 0;
        }
    });

    summaryEl.innerHTML = "";

    const cards = [
        {
            title: "Test Cases",
            value: total,
            extra: "Total"
        },
        {
            title: "Passed / Failed",
            value: `${passedCount}/${total - passedCount}`,
            extra: `${((passedCount / total) * 100).toFixed(1)}% Success`
        },
        {
            title: "Total Tool Invocations",
            value: totalInvokedCount,
            extra: "Tool calls made"
        },
        {
            title: "Wrong Invocations",
            value: totalWrongInvocations,
            extra: "Incorrect tool calls"
        },
        {
            title: "Total Replans",
            value: totalReplanCount,
            extra: "Times replanned"
        }
    ];

    cards.forEach(c => {
        const div = document.createElement("div");
        div.className = "card";
        div.innerHTML = `
            <div class="card-title">${c.title}</div>
            <div class="card-value">${c.value}</div>
            <div class="card-extra">${c.extra}</div>
        `;
        summaryEl.appendChild(div);
    });
}

// ===== Download JSON =====
function downloadJson() {
    if (!data) {
        alert("No data loaded. Please load data first.");
        return;
    }

    const blob = new Blob([JSON.stringify(data, null, 2)], {
        type: "application/json"
    });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = `eval-report-${new Date().toISOString().split('T')[0]}.json`;
    link.click();
    URL.revokeObjectURL(url);
}

// ===== Update Last Updated =====
function updateLastUpdated() {
    if (lastUpdatedEl) {
        const now = new Date();
        const timeStr = now.toLocaleString("en-US");
        lastUpdatedEl.textContent = `Last updated: ${timeStr}`;
    }
}

// ===== Helpers =====
function htmlEscape(str) {
    if (str == null) return "";
    return String(str)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#39;");
}

// ===== Event Listeners =====
if (loadBtn) {
    loadBtn.addEventListener("click", loadDataFromApi);
}

if (downloadJsonBtn) {
    downloadJsonBtn.addEventListener("click", downloadJson);
}

if (filterStatusSelect) {
    filterStatusSelect.addEventListener("change", filterAndRenderData);
}

if (searchBox) {
    searchBox.addEventListener("input", filterAndRenderData);
}

if (prevPageBtn) {
    prevPageBtn.addEventListener("click", () => {
        currentPage--;
        renderTablePage();
    });
}

if (nextPageBtn) {
    nextPageBtn.addEventListener("click", () => {
        currentPage++;
        renderTablePage();
    });
}

if (rowsPerPageSelect) {
    rowsPerPageSelect.addEventListener("change", (e) => {
        rowsPerPage = parseInt(e.target.value);
        currentPage = 1;
        renderTablePage();
    });
}

// Export for onclick handlers
window.downloadJson = downloadJson;
