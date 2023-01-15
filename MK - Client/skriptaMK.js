var host = "https://localhost:";
var port = "44353/";
var festivalEndpoint = "api/festivali/";
var mestoEndpoint = "api/mesta/";
var pretragaEndpoint = "api/festivali/pretraga";
var loginEndpoint = "api/authentication/login";
var registerEndpoint = "api/authentication/register";
var formAction = "Create";
var editingId;
var jwt_token;

function loadPage() {
  loadFestival();
  document.getElementById("loginForm").style.display = "block";
  document.getElementById("data").style.display = "block";
}

function showLogin() {
  document.getElementById("data").style.display = "none";
  document.getElementById("formDiv").style.display = "none";
  document.getElementById("loginForm").style.display = "block";
  document.getElementById("registerForm").style.display = "none";
  document.getElementById("logout").style.display = "none";
}

function loginUser() {
  var username = document.getElementById("usernameLogin").value;
  var password = document.getElementById("passwordLogin").value;

  if (validateLoginForm(username, password)) {
    var url = host + port + loginEndpoint;
    var sendData = { Username: username, Password: password };
    fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(sendData),
    })
      .then((response) => {
        if (response.status === 200) {
          alert("Uspešno logovanje!");
          response.json().then(function (data) {
            document.getElementById("info").innerHTML =
              "Prijavljen korisnik: <i>" + data.username + "<i/>.";
            document.getElementById("logout").style.display = "block";
            document.getElementById("data").style.display = "block";
            document.getElementById("pretragaDiv").style.display = "block";
            document.getElementById("formDiv").style.display = "block";
            document.getElementById("loginForm").style.display = "none";
            document.getElementById("registerForm").style.display = "none";
           
            jwt_token = data.token;
            loadFestival();
            loadMesto();
          });
        } else {
          console.log("Error occured with code " + response.status);
          console.log(response);
          alert("Desila se greška!");
        }
      })
      .catch((error) => console.log(error));
  }
  return false;
}

function validateLoginForm(username, password) {
  if (username.length === 0) {
    alert("Polje ne sme biti prazno.");
    return false;
  } else if (password.length === 0) {
    alert("Polje ne sme biti prazno.");
    return false;
  }
  return true;
}

function showRegistration() {
  document.getElementById("formDiv").style.display = "none";
  document.getElementById("loginForm").style.display = "none";
  document.getElementById("registerForm").style.display = "block";
  document.getElementById("data").style.display = "block";
  document.getElementById("logout").style.display = "none";
}

function registerUser() {
  var username = document.getElementById("usernameRegister").value;
  var email = document.getElementById("emailRegister").value;
  var password = document.getElementById("passwordRegister").value;
  var confirmPassword = document.getElementById(
    "confirmPasswordRegister"
  ).value;

  if (validateRegisterForm(username, email, password, confirmPassword)) {
    var url = host + port + registerEndpoint;
    var sendData = { Username: username, Email: email, Password: password };
    fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(sendData),
    })
      .then((response) => {
        if (response.status === 200) {
          alert("Uspešna registracija");
          showLogin();
        } else {
          alert("Desila se greška!");
        }
      })
      .catch((error) => console.log(error));
  }
  return false;
}

function validateRegisterForm(username, email, password, confirmPassword) {
  if (username.length === 0) {
    alert("Polje ne sme biti prazno.");
    return false;
  } else if (email.length === 0) {
    alert("Polje ne sme biti prazno.");
    return false;
  } else if (password.length === 0) {
    alert("Polje ne sme biti prazno.");
    return false;
  } else if (confirmPassword.length === 0) {
    alert("Polje ne sme biti prazno.");
    return false;
  } else if (password !== confirmPassword) {
    alert("Ovo polje mora da se podudara sa poljem lozinka.");
    return false;
  }
  return true;
}

function loadFestival() {
  var requestUrl = host + port + festivalEndpoint;
  var headers = {};
  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }
  fetch(requestUrl, { headers: headers })
    .then((response) => {
      if (response.status === 200) {
        response.json().then(setFestival);
      } else {
        console.log("Error occured with code " + response.status);
        showError();
      }
    })
    .catch((error) => console.log(error));
}

function setFestival(data) {
  var container = document.getElementById("data");
  container.innerHTML = "";

  console.log(data);

  var div = document.createElement("div");
  var h2 = document.createElement("h2");
  var headingText = document.createTextNode("Festivali");
  h2.appendChild(headingText);
  div.appendChild(h2);

  var table = document.createElement("table");
  table.className = "table table-bordered table-striped table-hover table-dark";
  var header = createHeader();
  table.append(header);

  var tableBody = document.createElement("tbody");

  for (var i = 0; i < data.length; i++) {
    var row = document.createElement("tr");

    row.appendChild(createTableCell(data[i].naziv));
    row.appendChild(createTableCell(data[i].mestoNaziv));
    row.appendChild(createTableCell(data[i].godina));
    row.appendChild(createTableCell(data[i].cena));

    if (jwt_token) {
      var stringId = data[i].id.toString();

      var buttonEdit = document.createElement("button");
      buttonEdit.name = stringId;
      buttonEdit.className = ("btn btn-warning");
      buttonEdit.addEventListener("click", editFestival);
      var buttonEditText = document.createTextNode("Izmeni");
      buttonEdit.appendChild(buttonEditText);
      var buttonEditCell = document.createElement("td");
      buttonEditCell.appendChild(buttonEdit);
      row.appendChild(buttonEditCell);

      var buttonEdit = document.createElement("button");
      buttonEdit.name = stringId;
      buttonEdit.className = ("btn btn-danger");
      buttonEdit.addEventListener("click", deleteFestival);
      var buttonDeleteText = document.createTextNode("Obriši");
      buttonEdit.appendChild(buttonDeleteText);
      var buttonDeleteCell = document.createElement("td");
      buttonDeleteCell.appendChild(buttonEdit);
      row.appendChild(buttonDeleteCell);
    }

    tableBody.appendChild(row);
  }

  table.appendChild(tableBody);
  div.appendChild(table);

  // if (jwt_token) {
  // 	document.getElementById("pretragaDiv").style.display = "block";
  // 	document.getElementById("formDiv").style.display = "block";
  // }

  container.appendChild(div);
}

function createHeader() {
  var thead = document.createElement("thead");
  var row = document.createElement("tr");

  row.appendChild(createTableHeaderCell("Naziv"));
  row.appendChild(createTableHeaderCell("Mesto"));
  row.appendChild(createTableHeaderCell("Godina"));
  row.appendChild(createTableHeaderCell("Cena"));

  if (jwt_token) {
    row.appendChild(createTableHeaderCell("Izmeni"));
    row.appendChild(createTableHeaderCell("Obriši"));
  }

  thead.appendChild(row);
  return thead;
}

function createTableCell(text) {
  var cell = document.createElement("td");
  var cellText = document.createTextNode(text);
  cell.appendChild(cellText);
  return cell;
}

function createTableHeaderCell(text) {
  var cell = document.createElement("th");
  var cellText = document.createTextNode(text);
  cell.appendChild(cellText);
  return cell;
}

function showError() {
  var container = document.getElementById("data");
  container.innerHTML = "";

  var div = document.createElement("div");
  var h1 = document.createElement("h1");
  var errorText = document.createTextNode(
    "Greška prilikom preuzimanja festivala!"
  );

  h1.appendChild(errorText);
  div.appendChild(h1);
  container.append(div);
}

function submitFestivalForm() {
  var festivalNaziv = document.getElementById("festivalNaziv").value;
  var festivalCena = document.getElementById("festivalCena").value;
  var festivalGodina = document.getElementById("festivalGodina").value;
  var festivalMesto = document.getElementById("festivalMesto").value;
  var httpAction;
  var sendData;
  var url;

  if (formAction === "Create") {
    httpAction = "POST";
    url = host + port + festivalEndpoint;
    sendData = {
      Naziv: festivalNaziv,
      Cena: festivalCena,
      Godina: festivalGodina,
      MestoId: festivalMesto,
    };
  } else {
    httpAction = "PUT";
    url = host + port + festivalEndpoint + editingId.toString();
    sendData = {
      Id: editingId,
      Naziv: festivalNaziv,
      Cena: festivalCena,
      Godina: festivalGodina,
      MestoId: festivalMesto,
    };
  }
  var headers = { "Content-Type": "application/json" };
  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }

  if (validateFestivalForm(festivalNaziv, festivalCena, festivalGodina)) {
    fetch(url, {
      method: httpAction,
      headers: headers,
      body: JSON.stringify(sendData),
    })
      .then((response) => {
        if (response.status === 200 || response.status === 201) {
          formAction = "Create";
          refreshTable();
        } else {
          alert("Desila se greška!");
        }
      })
      .catch((error) => console.log(error));
  }

  return false;
}

function validateFestivalForm(festivalNaziv, festivalCena, festivalGodina) {
  if (festivalNaziv.length === 0) {
    alert("Polje ne sme biti prazno.");
    return false;
  } else if (festivalCena <= 0) {
    alert("Cena ne sme biti manja od 0.");
    return false;
  } else if (festivalGodina < 1950 || festivalGodina > 2018) {
    alert("Opseg godina mora biti između 1950-2018.");
    return false;
  }

  return true;
}

function editFestival() {
	
	var editId = this.name;
	
	var url = host + port + festivalEndpoint + editId.toString();
	var headers = { };
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;		
	}
	fetch(url, { headers: headers})
		.then((response) => {
			if (response.status === 200) {
				response.json().then(data => {
					document.getElementById("festivalNaziv").value = data.naziv;
					document.getElementById("festivalCena").value = data.cena;
          document.getElementById("festivalGodina").value = data.godina;
          document.getElementById("festivalMesto").value = data.mestoId;
					editingId = data.id;
					formAction = "Update";
				});
			} else {
				formAction = "Create";
				alert("Desila se greška!");
			}
		})
		.catch(error => console.log(error));
};

function deleteFestival() {
  var deleteID = this.name;

  var url = host + port + festivalEndpoint + deleteID.toString();
  var headers = { "Content-Type": "application/json" };
  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }

  fetch(url, { method: "DELETE", headers: headers })
    .then((response) => {
      if (response.status === 204) {
        refreshTable();
      } else {
        alert("Desila se greška!");
      }
    })
    .catch((error) => console.log(error));
}

function loadMesto() {
  document.getElementById("data").style.display = "block";
  document.getElementById("loginForm").style.display = "none";
  document.getElementById("registerForm").style.display = "none";

  var requestUrl = host + port + mestoEndpoint;
  var headers = {};
  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }
  fetch(requestUrl, { headers: headers })
    .then((response) => {
      if (response.status === 200) {
        response.json().then(setMesto);
      } else {
        showError();
      }
    })
    .catch((error) => console.log(error));
}

function setMesto(data) {
  var dropdown = document.getElementById("festivalMesto");
  for (var i = 0; i < data.length; i++) {
    var option = document.createElement("option");
    option.value = data[i].id;
    var text = document.createTextNode(data[i].naziv);
    option.appendChild(text);
    dropdown.appendChild(option);
  }
}

function refreshTable() {
  document.getElementById("festivalNaziv").value = "";
  document.getElementById("festivalCena").value = "";
  document.getElementById("festivalGodina").value = "";
  loadFestival();
}

function cancel() {
  document.getElementById("festivalNaziv").value = "";
  document.getElementById("festivalCena").value = "";
  document.getElementById("festivalGodina").value = "";
  document.getElementById("festivalMesto").value = "";
  formAction = "Create";
}

function submitPretragaForm() {
  var start = document.getElementById("start").value;
  var kraj = document.getElementById("kraj").value;
  var sendData = {
    Start: start,
    Kraj: kraj,
  };
  var url = host + port + pretragaEndpoint;

  var headers = { "Content-Type": "application/json" };
  if (jwt_token) {
    headers.Authorization = "Bearer " + jwt_token;
  }

  if (validatePretragaForm(start, kraj)) {
  fetch(url, {
    method: "POST",
    headers: headers,
    body: JSON.stringify(sendData),
  })
    .then((response) => {
      if (response.status === 200) {
        response.json().then(setFestival);
        document.getElementById("pretragaForm").reset();
      } else {
        showError();
      }
    })
    .catch((error) => console.log(error));
  }
  return false;
}

function validatePretragaForm(start, kraj) {
  if (start < 1950) {
    alert("Početna godina ne sme biti manja od 1950!");
    return false;
  } else if (kraj > 2018) {
    alert("Krajnja godina ne sme biti veća od 2018!");
    return false;
  }

  return true;
}

function logout() {
  jwt_token = undefined;
  loadFestival();
  document.getElementById("info").innerHTML = "";
  document.getElementById("data").style.display = "none";
  document.getElementById("formDiv").style.display = "none";
  document.getElementById("loginForm").style.display = "block";
  document.getElementById("registerForm").style.display = "none";
  document.getElementById("logout").style.display = "none";
  document.getElementById("pretragaDiv").style.display = "none";
}
