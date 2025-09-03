use std::ffi::CString;
use std::ffi::c_char;
use rand::Rng;

// Basic symbol table that can be used within passwords
const SYMBOLS: [char; 32] = ['`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-',
                            '_', '+', '=', '[', ']', '{', '}', '\\', '|', ';', ':', '\'', '\'',
                            ',', '<', '.', '>', '/', '?'];

/**
 * Generates a list of the possible characters that can be used in the
 * user's final password.
 */
fn generate_char_list(no_symbols: bool, no_numbers: bool) -> Vec<char> {
    let mut char_list = Vec::new();

    char_list.extend('a'..='z');
    char_list.extend('A'..='Z');

    if !no_symbols {
        char_list.extend(SYMBOLS.iter());
    }

    if !no_numbers {
        char_list.extend('0'..='9');
    }

    char_list
}

/**
 * Creates the password from the valid vector list and accounts for uppercase or lowercase only selections.
 * Returns a normal Rust String.
 */
fn create_password(len: u8, char_list: Vec<char>, uppercase_only: bool, lowercase_only: bool) -> String {
    let mut password = String::new();
    let list_len = char_list.len();
    let mut rng = rand::rng();

    for _i in 0..len {
        // Get next char_list index randomly
        let index: usize = rng.random_range(0..list_len);
        // Add to password
        let mut inter_str: String = char_list[index].to_string();
        
        if inter_str.is_ascii() {
            if uppercase_only {
                inter_str = inter_str.to_uppercase();
            } else if lowercase_only {
                inter_str = inter_str.to_lowercase();
            }
        }

        password.push_str(inter_str.as_str());
    }

    password
}

/**
 * Generates a random password given the boolean selections from the user.
 * Returns a pointer to the string.
 */
#[unsafe(no_mangle)]
pub extern "C" fn generate_password(len: u8, no_symbols: bool, no_numbers: bool, uppercase_only: bool,lowercase_only: bool) -> *mut c_char {
    let char_list: Vec<char> = generate_char_list(no_symbols, no_numbers);
    let password = create_password(len, char_list, uppercase_only, lowercase_only);

    let c_pass_ptr: *mut c_char = CString::new(password).unwrap().into_raw();

    c_pass_ptr // Return the pointer to the c-string
}

/**
 * Frees memory of previously transferred CString.
 * Call this from C code to properly free memory.
 */
#[unsafe(no_mangle)]
pub extern "C" fn free_c_string(ptr: *mut c_char) {
    unsafe {
        let _  = CString::from_raw(ptr);
    }
}