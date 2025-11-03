import TodontList from "./components/TodontList";
import Header from "./components/Header";
import './App.css'

const App = () => {
    return (
        <div className="container">
            <Header />
            <main>
                <TodontList />
            </main>
        </div>
    );
};

export default App;
