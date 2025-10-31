import Todont from "./Todont";

const TodontList = () => {
  const items = [
    "Skip unnecessary meeting",
    "Ignore notifications",
    "Don't overthink",
  ];

  return (
    <ul>
      {items.map((text, i) => (
        <Todont key={i} title={text} />
      ))}
    </ul>
  );
}

export default TodontList;
