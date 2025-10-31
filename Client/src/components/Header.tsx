import '../styles/Header.css';

const Header = () => {
    return (
        <header className="header">
            <div className="header-logo">
                ToDon't
            </div>
            <div className="header-auth">
                <button
                    type="button"
                    className="header-login-button"
                >
                    Log In
                </button>
                <button
                    type="button"
                    className="header-signup-button"
                >
                    Sign Up
                </button>
            </div>
        </header>
    )
}

export default Header;
