const fName = document.querySelector("#fName");
const lName = document.querySelector("#lName");
const username = document.querySelector("#username");
const phone = document.querySelector("#phone");
const city = document.querySelector("#city");
const email = document.querySelector("#email");
const sBalance = document.querySelector("#sBalance");
const form = document.querySelector("#introForm");
const fNameErr = document.querySelector("#fNameError");
const lNameErr = document.querySelector("#lNameError");
const usernameErr = document.querySelector("#usernameError");
const phoneErr = document.querySelector("#phoneError");
const cityErr = document.querySelector("#cityError");
const emailErr = document.querySelector("#emailError");
const sBalanceErr = document.querySelector("#sBalanceError");

let profilePicNum = Math.floor(Math.random() * 15);
fName.addEventListener("change", isFNameValid);

function isFNameValid() {
  const regFName = /^[a-z][a-z\s'`-]{0,18}[^`]$/i;
  if (!fName.value || !regFName.test(fName.value)) {
    fNameErr.style.visibility = "visible";
    return false;
  }
  fNameErr.style.visibility = "hidden";
  return true;
}

lName.addEventListener("change", isLNameValid);

function isLNameValid() {
  const regLName = /^[a-z][a-z\s'`-]{0,28}[^`]$/i;
  if (!lName.value || !regLName.test(lName.value)) {
    lNameErr.style.visibility = "visible";
    return false;
  }
  lNameErr.style.visibility = "hidden";
  return true;
}

username.addEventListener("change", isUsernameValid);

function isUsernameValid() {
  const regUsername = /^[A-Z][a-z]{3}[0-5]$/;
  if (!username.value || !regUsername.test(username.value)) {
    usernameErr.style.visibility = "visible";
    return false;
  }
  usernameErr.style.visibility = "hidden";
  return true;
}

phone.addEventListener("change", isPhoneValid);

function isPhoneValid() {
  const regPhone = /^\([0-9]{3}\)\s[0-9]{3}-[0-9]{4}$/;
  if (!phone.value || !regPhone.test(phone.value)) {
    phoneErr.style.visibility = "visible";
    return false;
  }
  phoneErr.style.visibility = "hidden";
  return true;
}

city.addEventListener("change", isCityValid);

function isCityValid() {
  const regCity = /^[a-z]{1,42}$/i;
  if (!city.value || !regCity.test(city.value)) {
    cityErr.style.visibility = "visible";
    return false;
  }
  cityErr.style.visibility = "hidden";
  return true;
}

email.addEventListener("change", isEmailValid);

function isEmailValid() {
  const regEmail = /^[a-z0-9_\-.]+@[a-z0-9_]+.(ca|org)$/i;
  if (!email.value || !regEmail.test(email.value)) {
    emailErr.style.visibility = "visible";
    return false;
  }
  emailErr.style.visibility = "hidden";
  return true;
}

sBalance.addEventListener("change", isSBalanceValid);

function isSBalanceValid() {
  const regSBalance = /^[0-9]{1,4}$/;
  let bal = Number(sBalance.value);
  if (!sBalance.value || !regSBalance.test(sBalance.value) || bal % 3 != 0 || bal < 5 || bal > 5000) {
    sBalanceErr.style.visibility = "visible";
    return false;
  }
  sBalanceErr.style.visibility = "hidden";
  return true;
}

function areInputsValid() {
  return isFNameValid() && isLNameValid() && isUsernameValid() && isPhoneValid() && isCityValid() && isEmailValid() && isSBalanceValid();
}

form.addEventListener("submit", (e) => {
  isFNameValid();
  isLNameValid();
  isUsernameValid();
  isPhoneValid();
  isCityValid();
  isEmailValid();
  isSBalanceValid();
  if (areInputsValid()) {
    storeInputs();
    form.submit();
  }
  e.preventDefault();
});

function storeInputs() {
  localStorage.setItem("firstName", fName.value);
  localStorage.setItem("lastName", lName.value);
  localStorage.setItem("username", username.value);
  localStorage.setItem("phoneNumber", phone.value);
  localStorage.setItem("city", city.value);
  localStorage.setItem("email", email.value);
  localStorage.setItem("bank", sBalance.value);
  localStorage.setItem("profilePicNum", profilePicNum);
}
